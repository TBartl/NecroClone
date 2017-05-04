using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum OccupantId : byte {
    none,
    wall, player, enemy,
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
