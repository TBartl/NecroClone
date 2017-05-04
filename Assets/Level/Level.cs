﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
    public Tile[,] tiles;
    [HideInInspector] public IntVector2 size;
    [HideInInspector] public List<IntVector2> spawnPositions = new List<IntVector2>();
    [HideInInspector] public Transform parent;
    

    public Level(IntVector2 size) {
        this.size = size;
        tiles = new Tile[size.x, size.y];
    }

    public void Draw() {
        parent = new GameObject("Level").transform;
        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                tiles[x, y].Draw(new IntVector2(x, y), parent);
            }
        }
    }

    public bool InBounds(IntVector2 pos) {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y);
    }

    public bool Occuppied(IntVector2 pos) {
        if (!InBounds(pos))
            return true;
        return tiles[pos.x, pos.y].occupant != null;
    }

    public IntVector2 GetOpenPlayerSpawnPosition() {
        foreach (IntVector2 pos in ShuffleSpawnPositions()) {
            if (Occuppied(pos) == false) {
                return pos;
            }
        }
        return IntVector2.error;
    }
    List<IntVector2> ShuffleSpawnPositions() {
        List<IntVector2> copy = new List<IntVector2>(spawnPositions);
        for (int i = 0; i < copy.Count; i++) {
            IntVector2 temp = copy[i];
            int randomIndex = Random.Range(i, copy.Count);
            copy[i] = copy[randomIndex];
            copy[randomIndex] = temp;
        }
        return copy;
    }

    public GameObject AddOccupant(OccupantId id, IntVector2 pos) {
        GameObject prefab = LevelDatabase.S.GetOccupantPrefab(id);
        if (prefab == null || Occuppied(pos)) {
            Debug.LogError("Error adding occupant!");
            return null;
        }

        GameObject instance = (GameObject)GameObject.Instantiate(prefab, LevelManager.S.level.parent);
        tiles[pos.x, pos.y].occupant = instance;
        instance.transform.position = (Vector3)pos;

        IntVectorPos mov = instance.GetComponent<IntVectorPos>();
        if (mov) {
            mov.SetPos(pos);
        }

        return instance;
    }

    public GameObject GetOccupantAt(IntVector2 pos) {
        if (!InBounds(pos))
            return null;
        return tiles[pos.x, pos.y].occupant;
    }
}
