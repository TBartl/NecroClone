using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelManager : NetworkBehaviour {

    public static LevelManager S;
    public GameObject levelPrefab;
    

    [HideInInspector] public Level level;
    public LevelGenerator levelGenerator;

    void Awake() {
        S = this;
    }

    void Start() {
        if (isServer) {
            GameObject g = (GameObject)Instantiate(levelPrefab);
        }
    }

    IEnumerator WaitToStart() {
        yield return null;
    }
}
