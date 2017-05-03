using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager S;
    
    [HideInInspector] public Level level;
    public LevelGenerator generator;
    [HideInInspector] public LevelSerializer serializer = new LevelSerializer();

    void Awake() {
        S = this;
        NetManager.S.onNetworkSetup += OnNetworkSetup;
    }

    void OnNetworkSetup(bool isServer) {
        if (isServer) {
            level = generator.GetLevel();
            level.Draw();
        }
    }
}


//[Command]
//public void CmdSpawnPlayer() {
//    if (playerInstance != null)
//        return;
//    foreach (IntVector2 pos in LevelManager.S.level.spawnPositions) {
//        if (LevelManager.S.level.Occuppied(pos) == false) {
//            playerInstance = (GameObject)GameObject.Instantiate(playerPrefab, LevelManager.S.level.parent);
//            Movable mov = playerInstance.GetComponent<Movable>();
//            mov.SetPos(pos);
//            NetworkServer.Spawn(playerInstance);
//            break;
//        }
//    }
//}