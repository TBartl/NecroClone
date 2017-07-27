using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAutoHitDigOrMove {

    ActionAuto<ActionHit> hit;
    ActionAuto<ActionDig> dig;
    ActionAuto<ActionTryMove> move;


    public ActionAutoHitDigOrMove(GameObject owner) {
        hit = new ActionAuto<ActionHit>(owner);
        dig = new ActionAuto<ActionDig>(owner);
        move = new ActionAuto<ActionTryMove>(owner);
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

    public bool WillBump(IntVector2 direction) {
        if (hit.Exists() && hit.GetAction().CanHit(direction)) {
            return false;
        }
        else if (dig.Exists() && dig.GetAction().CanDig(direction)) {
            return false;
        }
        else if (move.Exists() && move.GetAction().CanMove(direction)) {
            return false;
        }
        return true;
    }
}