using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NetMessage_ClientConnectionID : NetMessage {

	public int id;

	public List<ClientData> clients;

	public NetMessage_ClientConnectionID() { }
	public NetMessage_ClientConnectionID(int id, List<ClientData> clients) {
		this.id = id;
		this.clients = clients;
	}

	protected override void EncodeToBuffer(ref BinaryWriter writer) {
		writer.Write(id);
		writer.Write(clients.Count);

		foreach (ClientData client in clients) {
			client.Encode(ref writer);
		}
	}

	protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
		id = reader.ReadInt32();
		NetManager.S.myConnectionId = id;

		int numClients = reader.ReadInt32();
		for (int i = 0; i < numClients; i++) {
			NetManager.S.AddClient(ClientData.Decode(ref reader));
		}
	}
}
