using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {
    public int detectionRadius = 0;

    GameObject closestTarget = null;

    protected override void Awake() {
        base.Awake();
        intTransform = this.GetComponent<IntTransform>();
    }

    protected virtual void OnReadyForNextAction(GameObject target) {
    }


    protected override void OnRecoverFinished() {
        if (closestTarget) {
            OnReadyForNextAction(closestTarget);
        }
    }

    public virtual void OnPlayerMoved(PlayerController player) {
        GameObject lastClosest = closestTarget;
        
        IntVector2 thisPos = intTransform.GetPos();
        int distToPlayer = IntVector2.ManDist(thisPos, player.GetComponent<IntTransform>().GetPos());
        if (distToPlayer > detectionRadius)
            return;

        if (closestTarget == null) {
            closestTarget = player.gameObject;
        }
        else {
            int currentDist = IntVector2.ManDist(thisPos, closestTarget.GetComponent<IntTransform>().GetPos());
            
            if (distToPlayer < currentDist) {
                closestTarget = player.gameObject;
            }
        }

        if (closestTarget != lastClosest && !IsRecovering()) {
            OnReadyForNextAction(closestTarget);
        }
    }
}
