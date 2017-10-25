using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public enum ConnectionGroup {
	lobby, game, both
}

public class ClientData {
    public int connectionID;
	public bool inGame;
	public string name;
    public GameObject player;

    public ClientData(int connectionID, bool inGame = false, string name = "Unknown") {
        this.connectionID = connectionID;
		this.inGame = inGame;
		this.name = name;
    }

	public void Encode(ref BinaryWriter writer) {
		writer.Write(connectionID);
		writer.Write(inGame);
		writer.Write(name);
	}

	public static ClientData Decode(ref BinaryReader reader) {
		ClientData client = new ClientData(reader.ReadInt32(), reader.ReadBoolean(), reader.ReadString());
		return client;
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

	List<ClientData> clients;
	public delegate void OnChange();
	public OnChange onClientChange = delegate { };

    [HideInInspector] public bool isConnected = false;

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
		// Reset
		myConnectionId = -1;
		clients = new List<ClientData>();
		onClientChange();

		// Try the connection
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
			AddClient(new ClientData(-1, false, "SERVERTODO"));
		}
        else {
            byte error;
            serverConnectionID = NetworkTransport.Connect(hostID, address, socketPort, 0, out error);
            if (error != (byte)NetworkError.Ok)
                Debug.LogWarning("Warning, potentially caught a network connection error!");
        }
        isConnected = true;

		// Load the lobby scene
		SceneManager.LoadScene(1);
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

		// Send it out
		foreach (ClientData client in clients) {
			if (group == ConnectionGroup.lobby && client.inGame)
				continue;
			if (group == ConnectionGroup.game && !client.inGame)
				continue;

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
		SendServerMessageToOne(new NetMessage_ClientConnectionID(connectionId, clients), connectionId);
		ClientData newClient = new ClientData(connectionId, false, "unknown");
		AddClient(newClient);
		SendServerMessageToGroup(new NetMessage_AddClient(newClient), ConnectionGroup.both);
	}

    void OnClientDisconnect(int connectionId) {
		RemoveClient(connectionId);
		SendServerMessageToGroup(new NetMessage_RemoveClient(connectionId), ConnectionGroup.both);
	}

    void HandleDataMessage(int connectionId) {
        NetMessage message = NetMessageMaintainer.GetFromRecognizeByte(NetMessage.buffer[0]);
        message.DecodeAndExecute(GetClientById(connectionId));
    }

	public ClientData GetThisServerClient() {
		return GetClientById(-1);
	}

	public ClientData GetClientById (int id) {
        foreach (ClientData client in clients) {
            if (client.connectionID == id)
                return client;
        }
        return null;
    }

	public List<ClientData> GetClients() {
		return clients;
	}
	public void AddClient(ClientData client) {
		clients.Add(client);
		onClientChange();
	}
	public void RemoveClient(int id) {
		clients.Remove(GetClientById(id));
		onClientChange();
	}
}