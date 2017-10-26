using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class NetMessage_StartGame : NetMessage {

	string sceneName;

	public NetMessage_StartGame() { }
	public NetMessage_StartGame(string sceneName) {
		this.sceneName = sceneName;
	}

	public override bool AlsoExecuteOnServer() {
		return true;
	}

	protected override void EncodeToBuffer(ref BinaryWriter writer) {
		writer.Write(sceneName);
	}

	protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
		sceneName = reader.ReadString();
		foreach (ClientData client in NetManager.S.GetClients()) {
			client.inGame = true;
		}
		SceneManager.LoadScene(sceneName);
	}
}
