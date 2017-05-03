using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelDatabase_Floors {
    public GameObject basic;
}

[System.Serializable]
public struct LevelDatabase_Occupants {
    public GameObject wall;
    public GameObject player;
    public GameObject enemy;
}

public class LevelDatabase : MonoBehaviour {

    public static LevelDatabase S;

    GameObject[] floors = new GameObject[(byte)FloorId.count];
    GameObject[] occupants = new GameObject[(byte)OccupantId.count];

    void Awake() {
        S = this;
        ResourceLoadFolder("Floors",    ref floors);
        ResourceLoadFolder("Occupants", ref occupants);
    }

    void ResourceLoadFolder(string folderName, ref GameObject[] list) {
        GameObject[] unordered = Resources.LoadAll<GameObject>(folderName);
        foreach (GameObject g in unordered) {
            byte id = g.GetComponent<DatabaseID>().GetID();
            list[id] = g;
        }
    }

    public GameObject GetFloorPrefab(FloorId floorId) {
        return floors[(byte)floorId];
    }
    public GameObject GetOccupantPrefab(OccupantId occupantId) {
        return occupants[(byte)occupantId];
    }
}
