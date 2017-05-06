using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class IntTransform : MonoBehaviour {

    public delegate void OnMove(IntVector2 from, IntVector2 to);
    public OnMove onRealMove;
    public OnMove onBumpMove;
    IntVector2 pos;
    Level level;

    void Awake() {
        level = this.GetComponentInParent<Level>();
    }
    
    public void SetPos(IntVector2 pos) {
        this.pos = pos;
        this.transform.position = (Vector3)pos;
    }
    public IntVector2 GetPos() {
        return pos;
    }

    public bool CanMove(IntVector2 newPos) {
        if (level.Occuppied(newPos))
            return false;
        return true;
    }

    public bool TryMove(IntVector2 newPos) {
        if (!CanMove(newPos)) {
            onBumpMove(pos, newPos);
            return false;
        }

        this.transform.position = (Vector3)newPos;
        onRealMove(pos, newPos);
        level.tiles[pos.x, pos.y].occupant = null;
        level.tiles[newPos.x, newPos.y].occupant = this.gameObject;
        pos = newPos;
        return true;
    }
    public void Bump(IntVector2 to) {
        onBumpMove(pos, to);
    }

    public Level GetLevel() {
        return level;
    }
}
