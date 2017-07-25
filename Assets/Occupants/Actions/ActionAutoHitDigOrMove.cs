using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAutoHitDigOrMove {

    ActionAuto<ActionHit> hit;
    ActionAuto<ActionDig> dig;
    ActionAuto<ActionMove> move;


    public ActionAutoHitDigOrMove(GameObject owner) {
        hit = new ActionAuto<ActionHit>(owner);
        dig = new ActionAuto<ActionDig>(owner);
        move = new ActionAuto<ActionMove>(owner);
    }

    public Action GetAction(IntVector2 direction) {
        if (hit.Exists() && hit.GetAction().CanHit(direction)) {
            return hit.GetAction();
        }
        else if (dig.Exists() && dig.GetAction().CanDig(direction)) {
            return dig.GetAction();
        }
        else if (move.Exists() /*&& move.CanMove(direction)*/) {
            return move.GetAction();
        }
        return null;
    }
}