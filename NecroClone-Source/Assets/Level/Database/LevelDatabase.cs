﻿using System.Collections;
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

    Dictionary<string, GameObject> floors = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> occupants = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> collectables = new Dictionary<string, GameObject>();

    void Awake() {
		if (S != null)
			return;
		S = this;
        ResourceLoadFolder("Floors",    ref floors);
		ResourceLoadFolder("Occupants", ref occupants);
		ResourceLoadFolder("Collectables", ref collectables);
	}

    void ResourceLoadFolder(string folderName, ref Dictionary<string, GameObject> dictionary) {
        GameObject[] unordered = Resources.LoadAll<GameObject>(folderName);
        foreach (GameObject g in unordered) {
            string name = g.name;
            dictionary[name] = g;
        }
    }

    public GameObject GetFloorPrefab(string name) {
        if (name == "")
            return null;
        if (!floors.ContainsKey(name)) {
            return null;
        }
        return floors[name];
    }
    public GameObject GetOccupantPrefab(string name) {
        if (name == "")
            return null;
        if (!occupants.ContainsKey(name)) {
            return null;
        }
        return occupants[name];
    }
	public GameObject GetCollectablePrefab(string name) {
		if (name == "")
			return null;
		if (!collectables.ContainsKey(name)) {
			return null;
		}
		return collectables[name];
	}
	public GameObject GetItemPrefab() {
		return GetCollectablePrefab("Item");
	}
}
