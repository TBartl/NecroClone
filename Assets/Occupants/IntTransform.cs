using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class IntTransform : MonoBehaviour {

    //TODO remove
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

    public bool CanOccupy(IntVector2 newPos) {
        if (level.Occuppied(newPos))
            return false;
        return true;
    }

    public void UnOccupyCurrentPos() {
        if (level.tiles[pos.x, pos.y].occupant != this.gameObject)
            Debug.LogError("Trying to unoccupy when already not occupying");
        level.tiles[pos.x, pos.y].occupant = null;
    }

    public void OccupyCurrentPos() {
        if (!CanOccupy(pos))
            Debug.LogError("Trying to occupy an occupied spot!");
        level.tiles[pos.x, pos.y].occupant = this.gameObject;
    }

    public Level GetLevel() {
        return level;
    }
}
