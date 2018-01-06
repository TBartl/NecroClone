using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyStartLevel : MonoBehaviour {

	public string sceneName;

	public void OnClick() {
		if (!NetManager.S.isServer)
			return;
		NetManager.S.SendServerMessageToGroup(new NetMessage_StartGame(sceneName), ConnectionGroup.both);
	}
}