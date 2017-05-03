using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager S;

    [HideInInspector]
    public Level level;
    public LevelGenerator generator;
    [HideInInspector]
    public LevelSerializer serializer = new LevelSerializer();

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
