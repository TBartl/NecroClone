using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelDatabase_Floor {
    public GameObject basic;
}

[System.Serializable]
public struct LevelDatabase_Walls {
    public GameObject basic;
}

[System.Serializable]
public struct LevelDatabase_Entities {
    public GameObject player;
}


public class LevelDatabase : MonoBehaviour {

    public static LevelDatabase S;

    public LevelDatabase_Floor floor;
    public LevelDatabase_Walls walls;
    public LevelDatabase_Entities entities;

    void Awake() {
        S = this;
    }
}
