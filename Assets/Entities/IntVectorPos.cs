using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IntVectorPos : MonoBehaviour {

    public MoveAnimationData animationData;
    IntVector2 pos;
    
    public void SetPos(IntVector2 pos) {
        this.pos = pos;
        this.transform.position = (Vector3)pos;
    }
    public IntVector2 GetPos() {
        return pos;
    }

    public bool CanMove(IntVector2 newPos) {
        if (LevelManager.S.level.Occuppied(newPos))
            return false;
        return true;
    }

    public bool TryMove(IntVector2 newPos) {
        if (!CanMove(newPos)) {
            StartCoroutine(JuicyFailMove(pos, newPos));
            return false;
        }

        StartCoroutine(JuicyMove(pos, newPos));
        LevelManager.S.level.tiles[pos.x, pos.y].occupant = null;
        LevelManager.S.level.tiles[newPos.x, newPos.y].occupant = this.gameObject;
        pos = newPos;
        return true;
    }

    IEnumerator JuicyMove(IntVector2 from, IntVector2 to) {
        for (float t = 0; t < animationData.animLength; t += Time.deltaTime) {
            float p = t / animationData.animLength;
            Vector3 intermediatePos = Vector3.Lerp((Vector3)from, (Vector3)to, animationData.horizontal.Evaluate(p));
            intermediatePos.y = animationData.vertical.Evaluate(p);
            this.transform.position = intermediatePos;
            yield return null;
        }
        this.transform.position = (Vector3)to;        
    }

    public void Bump(IntVector2 to) {
        StartCoroutine(JuicyFailMove(pos, to));
    }

    IEnumerator JuicyFailMove(IntVector2 from, IntVector2 to) {
        for (float t = 0; t < animationData.animLength; t += Time.deltaTime) {
            float p = t / animationData.animLength;
            Vector3 intermediatePos = Vector3.Lerp((Vector3)from, (Vector3)to, animationData.horizontalFail.Evaluate(p));
            intermediatePos.y = animationData.vertical.Evaluate(p);
            this.transform.position = intermediatePos;
            yield return null;
        }
        this.transform.position = (Vector3)from;
    }
}
