using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public static Player S;
    public GameObject playerPrefab;
    GameObject playerInstance;

    public override void OnStartClient() {
        S = this;
    }

    void Update() {
        if (!isLocalPlayer)
            return;

        if (playerInstance == null) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                CmdSpawnPlayer();
            }
        }
    }

    [Command]
    public void CmdSpawnPlayer() {
        if (playerInstance != null)
            return;
        foreach(IntVector2 pos in LevelManager.S.level.spawnPositions) {
            if (LevelManager.S.level.Occuppied(pos) == false) {
                playerInstance = (GameObject)GameObject.Instantiate(playerPrefab, LevelManager.S.level.parent);
                Movable mov = playerInstance.GetComponent<Movable>();
                mov.SetPos(pos);
                NetworkServer.Spawn(playerInstance);
                break;
            }
        }
    }
}
