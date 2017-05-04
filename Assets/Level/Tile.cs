using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile {
    public GameObject floor;
    public GameObject occupant;

    public void Draw(IntVector2 position, Transform parent) {
        if (floor != null)
            floor = (GameObject)GameObject.Instantiate(floor, (Vector3)position, Quaternion.identity, parent);
        if (occupant != null) {
            occupant = (GameObject)GameObject.Instantiate(occupant, (Vector3)position, Quaternion.identity, parent);
            IntVectorPos mov = occupant.GetComponent<IntVectorPos>();
            if (mov) {
                mov.SetPos(position);
            }
        }
    }
}
