using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum ConnectionGroup {
	lobby, game, both
}

public class ClientData {
    public int connectionID;
	public string name;
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

	public int myConnectionId;

	List<ClientData> lobbyClients;
	List<ClientData> gameClients;

    [HideInInspector] public bool isConnected = false;
    public delegate void OnNetworkSetup(bool isServer);
    public OnNetworkSetup onNetworkSetup;

    void Awake() {
		if (S != null)
			return;
		S = this;
        NetMessageMaintainer.Setup();
    }

	public void StartHost() {
        isServer = true;
		Setup();
	}

    public void StartConnect(string address = "") {
        if (address == "")
            this.address = "127.0.0.1";
        else
            this.address = address;
        isServer = false;
		Setup();
	}

    void Setup() {
		//Reset
		myConnectionId = -1;
		lobbyClients = new List<ClientData>();
		gameClients = new List<ClientData>();

		SceneManager.LoadScene(1);
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
			lobbyClients.Add(new ClientData(-1));
        }
        else {
            byte error;
            serverConnectionID = NetworkTransport.Connect(hostID, address, socketPort, 0, out error);
            if (error != (byte)NetworkError.Ok)
                Debug.LogWarning("Warning, potentially caught a network connection error!");
        }
        isConnected = true;
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

	public void SendServerMessageToGroup(NetMessage message, ConnectionGroup group) {
		int messageLength = message.Encode();
		byte error;

		List<ClientData> clients = new List<ClientData>();
		if (group != ConnectionGroup.game)
			clients.AddRange(lobbyClients);
		else if (group != ConnectionGroup.lobby)
			clients.AddRange(gameClients);

		// Send it out
		foreach (ClientData client in clients) {
			if (client.connectionID == -1) {
				if (message.AlsoExecuteOnServer()) {
					message.DecodeAndExecute(client);
				}
			}
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
                    break;
                case NetworkEventType.DataEvent:
                    HandleDataMessage(connectionId);
                    break;
                case NetworkEventType.DisconnectEvent:
                    OnClientDisconnect(connectionId);
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
                    break;
                case NetworkEventType.DataEvent:
                    HandleDataMessage(connectionId);
                    break;
                case NetworkEventType.DisconnectEvent:
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
        lobbyClients.Add(new ClientData(connectionId));

		//LevelManager.S.serializer.Serialise(LevelManager.S.startLevel);
		SendServerMessageToOne(new NetMessage_ClientConnectionID(connectionId), connectionId);
		////TODO send all the levels, not just start
		//SendServerMessageToOne(new NetMessage_StartSendLevel(LevelManager.S.startLevel), connectionId);
		//for (int i = 0; i < LevelManager.S.serializer.GetRequiredNumOfPieces(); i++) {
		//    SendServerMessageToOne(new NetMessage_SendLevelPiece(i), connectionId);
		//}
	}

    void OnClientDisconnect(int connectionId) {
        ClientData client = GetClientById(connectionId);
		lobbyClients.Remove(client);
		gameClients.Remove(client);
	}

    void HandleDataMessage(int connectionId) {
        NetMessage message = NetMessageMaintainer.GetFromRecognizeByte(NetMessage.buffer[0]);
        message.DecodeAndExecute(GetClientById(connectionId));
    }

	public ClientData GetThisServerClient() {
		return GetClientById(-1);
	}


	public ClientData GetClientById (int id) {
        foreach (ClientData client in GetClients()) {
            if (client.connectionID == id)
                return client;
        }
        return null;
    }

    public List<ClientData> GetClients() {
		List<ClientData> clients = new List<ClientData>();
		clients.AddRange(lobbyClients);
		clients.AddRange(gameClients);
        return clients;
    }
}