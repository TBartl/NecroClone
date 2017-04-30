using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Movable : NetworkBehaviour {
    IntVector2 pos;
    
    public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        if (!initialState)
            return false;
        else
            return base.OnSerialize(writer, true);
    }
    public override void OnDeserialize(NetworkReader reader, bool initialState) {
        if (!initialState)
            return;
        pos.x = (int)reader.ReadPackedUInt32();
        pos.y = (int)reader.ReadPackedUInt32();
    }

    public override void OnStartClient() {
        this.transform.position = (Vector3)pos;
    }

    
    
    public void SetPos(IntVector2 pos) {
        this.pos = pos;
    }
    public IntVector2 GetPos() {
        return pos;
    }

    public bool TryMove(IntVector2 newPos) {
        if (LevelManager.S.level.Occuppied(newPos))
            return false;
        LevelManager.S.level.tiles[pos.x, pos.y].occupant = null;
        LevelManager.S.level.tiles[newPos.x, newPos.y].occupant = this.gameObject;
        pos = newPos;
        this.transform.position = (Vector3)newPos;
        return true;
    }
}
