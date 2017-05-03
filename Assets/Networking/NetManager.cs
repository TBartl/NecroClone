using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetManager : MonoBehaviour {

    public static NetManager S;
    bool isServer = false;
    public int maxConnections = 10;
    public string address = "127.0.0.1";
    public int socketPort = 7777;

    int channelID;
    int hostID;
    int serverConnectionID;
    List<int> connectedClients = new List<int>();

    NetMessage[] messageTypes = {
        new NetMessage(), new NetMessageDebug(),
        new NetMessage_StartSendLevel(), new NetMessage_SendLevelPiece()};

    [HideInInspector] public bool isConnected = false;
    public delegate void OnNetworkSetup(bool isServer);
    public OnNetworkSetup onNetworkSetup;


    void Awake() {
        if (S == null)
            S = this;
        SceneManager.activeSceneChanged += SceneChanged;
    }

    public void StartHost() {
        isServer = true;
        SceneManager.LoadScene(1);
    }

    public void StartConnect() {
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

        if (!isServer) {
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

            if (!isServer && Input.GetKeyDown(KeyCode.Space))
                SendNetMessage(new NetMessageDebug("TEST"), serverConnectionID);
        }
    }

    public void SendNetMessage(NetMessage message, int connectionID) {
        List<int> list = new List<int>();
        list.Add(connectionID);
        SendNetMessage(message, list);
    }
    public void SendNetMessage(NetMessage message, List<int> connectionIDs) {
        message.EncodeToBuffer();
        byte error;

        // Execute it locally
        if (isServer && message.AlsoExecuteOnServer())
            message.DecodeBufferAndExecute();

        // Send it out
        /// TODO Optimize how much data we are sending so we don't just always send the full buffer
        foreach (int id in connectionIDs) {
            NetworkTransport.Send(hostID, id, channelID, NetMessage.buffer, NetMessage.bufferSize, out error);
        }

    }

    void UpdateServer() {
        int recHostId;
        int connectionId;
        int channelId;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, NetMessage.buffer, NetMessage.bufferSize, out dataSize, out error);
        switch (recData) {
            case NetworkEventType.Nothing:         //1
                break;
            case NetworkEventType.ConnectEvent:    //2
                OnClientConnect(connectionId);
                Debug.Log("Server Connect Event");
                break;
            case NetworkEventType.DataEvent:       //3
                Debug.Log("Server Data Event");
                HandleDataMessage(connectionId);
                break;
            case NetworkEventType.DisconnectEvent: //4
                Debug.Log("Server Disconnect Event");
                break;
        }
    }

    void UpdateClient() {
        int recHostId;
        int connectionId;
        int channelId;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, NetMessage.buffer, NetMessage.bufferSize, out dataSize, out error);
        switch (recData) {
            case NetworkEventType.Nothing:         //1
                break;
            case NetworkEventType.ConnectEvent:    //2
                Debug.Log("Client Connect Event");
                break;
            case NetworkEventType.DataEvent:       //3
                Debug.Log("Client Data Event");
                HandleDataMessage(connectionId);
                break;
            case NetworkEventType.DisconnectEvent: //4
                Debug.Log("Client Disconnect Event");
                break;
        }
    }

    void OnClientConnect(int connectionId) {
        connectedClients.Add(connectionId);
        LevelManager.S.serializer.Serialise();
        SendNetMessage(new NetMessage_StartSendLevel(), connectionId);
        Debug.Log(LevelManager.S.serializer.GetRequiredNumOfPieces());
        for (int i = 0; i < LevelManager.S.serializer.GetRequiredNumOfPieces(); i++) {
            SendNetMessage(new NetMessage_SendLevelPiece(i), connectionId);
        }

    }

    void HandleDataMessage(int connectionId) {
        foreach (NetMessage messageType in messageTypes) {
            if (messageType.IsThisMessage()) {
                messageType.DecodeBufferAndExecute();
            }
        }
    }
}