using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile {
    public GameObject floor;
    public GameObject occupant;

    public void Draw(IntVector2 position, Level level) {
        if (floor != null) {
            string nameWithoutClone = floor.name;
            floor = (GameObject)GameObject.Instantiate(floor, (Vector3)position, Quaternion.identity, level.transform);
            floor.name = nameWithoutClone;
        }
        if (occupant != null) {
            occupant = level.SpawnOccupant(occupant.name, position, true);
        }
    }
}
