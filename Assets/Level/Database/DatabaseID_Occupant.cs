using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum OccupantId : byte {
    none,
    wall, player,
    skeleton1, skeleton2, skeleton3,
    bat1, bat2,
    spawner, spawnerEnemy,
    count
}

public class DatabaseID_Occupant : DatabaseID {
    [SerializeField] OccupantId id;

    public override byte GetID() {
        return (byte)id;
    }
    public OccupantId GetOccupantID() {
        return id;
    }
}
