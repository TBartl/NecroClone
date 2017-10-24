using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ClientData {
    public int connectionID;
    public GameObject player;

    public ClientData(int connectionID) {
        this.connectionID = connectionID;
    }
}

public class NetManager : MonoBehaviour {

    public static NetManager S;
    [HideInInspector] public bool isServer = false;
    public int maxConnections = 10;
    public string address = "127.0.0.1";
    public int socketPort = 7777;

    int channelID;
    int hostID;
    int serverConnectionID;

    public int myConnectionId = -1; //-1 for server, something else for clients

    ClientData serverClient;
    List<ClientData> connectedClients = new List<ClientData>();

    [HideInInspector] public bool isConnected = false;
    public delegate void OnNetworkSetup(bool isServer);
    public OnNetworkSetup onNetworkSetup;

    void Awake() {
		if (S != null)
			return;
		S = this;
        SceneManager.activeSceneChanged += SceneChanged;
        NetMessageMaintainer.Setup();
    }

	public void ResetNetwork() {
		// TODO
	}

	public void StartHost() {
        isServer = true;
        SceneManager.LoadScene(1);
    }

    public void StartConnect(string address = "") {
        if (address == "")
            this.address = "127.0.0.1";
        else
            this.address = address;

        isServer = false;
        SceneManager.LoadScene(1);
    }

    public void SceneChanged(Scene from, Scene to) {
        if (to.buildIndex == 1) {
            Setup();
        }
    }

    void Setup() {
        isConnected = false;
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        channelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        if (isServer)
            hostID = NetworkTransport.AddHost(topology, socketPort, null);
        else
            hostID = NetworkTransport.AddHost(topology, 0);

        if (isServer) {
            serverClient = new ClientData(-1);
        }
        else {
            byte error;
            serverConnectionID = NetworkTransport.Connect(hostID, address, socketPort, 0, out error);
            if (error != (byte)NetworkError.Ok)
                Debug.LogWarning("Warning, potentially caught a network connection error!");
        }
        isConnected = true;
        onNetworkSetup(isServer);
    }

    void Update() {
        if (isConnected) {
            if (isServer)
                UpdateServer();
            else
                UpdateClient();
        }
    }

    public void SendClientMessage(NetMessage message) {
        int messageLength = message.Encode();

        if (isServer)
            message.DecodeAndExecute(GetThisServerClient());
        else {
            byte error;
            NetworkTransport.Send(hostID, serverConnectionID, channelID, NetMessage.buffer, messageLength, out error);
        }
    }

    public void SendServerMessageToOne(NetMessage message, int connectionID) {
        int messageLength = message.Encode();

        List<int> list = new List<int>();
        list.Add(connectionID);
        byte error;
        NetworkTransport.Send(hostID, connectionID, channelID, NetMessage.buffer, messageLength, out error);
    }
    public void SendServerMessageToAll(NetMessage message) {
        int messageLength = message.Encode();
        byte error;

        // Execute it locally
        if (isServer && message.AlsoExecuteOnServer())
            message.DecodeAndExecute(GetThisServerClient());

        // Send it out
        foreach (ClientData client in connectedClients) {
            NetworkTransport.Send(hostID, client.connectionID, channelID, NetMessage.buffer, messageLength, out error);
        }
    }

    void UpdateServer() {
        int recHostId;
        int connectionId;
        int channelId;
        int dataSize;
        byte error;

        bool breakOuterLoop = false;
        while (true) {
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, NetMessage.buffer, NetMessage.bufferSize, out dataSize, out error);
            switch (recData) {
                case NetworkEventType.ConnectEvent:
                    OnClientConnect(connectionId);
                    //Debug.Log("Server Connect Event");
                    break;
                case NetworkEventType.DataEvent:
                    //Debug.Log("Server Data Event");
                    HandleDataMessage(connectionId);
                    break;
                case NetworkEventType.DisconnectEvent:
                    OnClientDisconnect(connectionId);
                    //Debug.Log("Server Disconnect Event");
                    break;
                default:
                    breakOuterLoop = true;
                    break;
            }

            if (breakOuterLoop)
                break;
        }
    }

    void UpdateClient() {
        int recHostId;
        int connectionId;
        int channelId;
        int dataSize;
        byte error;

        bool breakOuterLoop = false;
        while (true) {
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, NetMessage.buffer, NetMessage.bufferSize, out dataSize, out error);
            switch (recData) {
                case NetworkEventType.ConnectEvent:
                    //Debug.Log("Client Connect Event");
                    break;
                case NetworkEventType.DataEvent:
                    //Debug.Log("Client Data Event");
                    HandleDataMessage(connectionId);
                    break;
                case NetworkEventType.DisconnectEvent:
                    //Debug.Log("Client Disconnect Event");
                    break;
                default:
                    breakOuterLoop = true;
                    break;
            }
            if (breakOuterLoop)
                break;
        }
    }

    void OnClientConnect(int connectionId) {
        connectedClients.Add(new ClientData(connectionId));
        LevelManager.S.serializer.Serialise(LevelManager.S.startLevel);
        SendServerMessageToOne(new NetMessage_ClientConnectionID(connectionId), connectionId);
        //TODO send all the levels, not just start
        SendServerMessageToOne(new NetMessage_StartSendLevel(LevelManager.S.startLevel), connectionId);
        for (int i = 0; i < LevelManager.S.serializer.GetRequiredNumOfPieces(); i++) {
            SendServerMessageToOne(new NetMessage_SendLevelPiece(i), connectionId);
        }
    }

    void OnClientDisconnect(int connectionId) {
        ClientData client = GetClientById(connectionId);
        //if (client.player)
        //    Destroy(client.player); Would need to write a NetMessage for this and it's debatable if we need this
        connectedClients.Remove(client);
    }

    void HandleDataMessage(int connectionId) {
        NetMessage message = NetMessageMaintainer.GetFromRecognizeByte(NetMessage.buffer[0]);
        message.DecodeAndExecute(GetClientById(connectionId));
    }

    public ClientData GetThisServerClient() {
        return serverClient;
    }

    public ClientData GetClientById (int id) {
        if (id == -1)
            return serverClient;
        foreach (ClientData client in connectedClients) {
            if (client.connectionID == id)
                return client;
        }
        return null;
    }

    public List<ClientData> GetClients() {
        return connectedClients;
    }
}