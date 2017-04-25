using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    IntVector2 pos;
    [HideInInspector] public IntVector2 targetPos;

    public void SetInitialPos(IntVector2 pos) {
        this.pos = pos;
    }

    public IntVector2 GetPos() {
        return pos;
    }

    public bool TryMove(IntVector2 newPos) {
        if (LevelManager.S.level.Occuppied(newPos))
            return false;
        pos = newPos;
        this.transform.position = (Vector3)newPos;
        return true;
    }
}
