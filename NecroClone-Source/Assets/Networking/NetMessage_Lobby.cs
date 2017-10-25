using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NetMessage_ClientConnectionID : NetMessage {

	public int id;

	public List<ClientData> lobbyClients;
	public List<ClientData> gameClients;

	public NetMessage_ClientConnectionID() { }
	public NetMessage_ClientConnectionID(int id, List<ClientData> lobbyClients, List<ClientData> gameClients) {
		this.id = id;
		this.lobbyClients = lobbyClients;
		this.gameClients = gameClients;
	}

	protected override void EncodeToBuffer(ref BinaryWriter writer) {
		writer.Write(id);
		writer.Write(lobbyClients.Count);
		foreach (ClientData client in lobbyClients) {
			WriteClientData(ref writer, client);
		}
		writer.Write(gameClients.Count);
		foreach (ClientData client in gameClients) {
			WriteClientData(ref writer, client);
		}
	}

	protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
		id = reader.ReadInt32();
		NetManager.S.myConnectionId = id;
		int numLobbyClients = reader.ReadInt32();
		for (int i = 0; i < numLobbyClients; i++) {
			//NetManager.S.AddLobbyClient(ReadClientData(ref reader));
		}
		int numGameClients = reader.ReadInt32();
		for (int i = 0; i < numGameClients; i++) {
			//NetManager.S.AddGameClient(ReadClientData(ref reader));
		}
	}

	void WriteClientData(ref BinaryWriter writer, ClientData client) {
		writer.Write(client.connectionID);
		writer.Write(client.name);

	}
	ClientData ReadClientData(ref BinaryReader reader) {
		ClientData client = new ClientData(reader.ReadInt32());
		client.name = reader.ReadString();
		return client;
	}
}

[System.Serializable]
public class NetMessage_SendName : NetMessage {

}
