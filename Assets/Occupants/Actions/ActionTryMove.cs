using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTryMove : ActionFixedRecoverTime {

    public bool CanMove(IntVector2 direction) {
        return intTransform.CanOccupy(intTransform.GetPos() + direction);
    }

    public override void Execute(IntVector2 direction) {
        IntVector2 oldPos = intTransform.GetPos();
        IntVector2 newPos = intTransform.GetPos() + direction;
        if (!CanMove(direction)) {
            Debug.Log("A");
            foreach (SmoothMove smoothMove in this.GetComponentsInChildren<SmoothMove>()) {
                smoothMove.Bump(newPos);
            }
        }
        else {
            Debug.Log("B");
            intTransform.UnOccupyCurrentPos();
            intTransform.SetPos(newPos);
            intTransform.OccupyCurrentPos();

            foreach (SmoothMove smoothMove in this.GetComponentsInChildren<SmoothMove>()) {
                smoothMove.Move(oldPos, newPos);
            }
        }
    }


}
