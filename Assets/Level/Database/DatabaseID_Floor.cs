using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum FloorId : byte {
    none,
    basic,
    count
}

public class DatabaseID_Floor : DatabaseID {
    [SerializeField] FloorId id;

    public override byte GetID() {
        return (byte)id;
    }
}
