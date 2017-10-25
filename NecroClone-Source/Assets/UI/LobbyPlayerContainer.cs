using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerContainer : MonoBehaviour {

	public static LobbyPlayerContainer S;

	public GameObject playerPrefab;

	public Text playerCountText;

	void Awake() {
		S = this;
	}

	void Start() {
		UpdateClients();
		NetManager.S.onClientChange += UpdateClients;
	}

	void UpdateClients() {
		List<ClientData> clients = NetManager.S.GetClients();
		playerCountText.text = string.Format("Players ({0}/{1})", clients.Count, NetManager.S.maxConnections);
		for (int i = 0; i < clients.Count; i++) {
			GameObject p = Instantiate(playerPrefab, transform);
			RectTransform rect = p.GetComponent<RectTransform>();
			rect = p.GetComponent<RectTransform>();
			rect.anchoredPosition = Vector2.down * rect.sizeDelta.y * i;
			p.GetComponent<Text>().text = string.Format("{0} - {1} {2}", i, clients[i].name, clients[i].inGame);
		}
	}
}
