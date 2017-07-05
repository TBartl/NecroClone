using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {
    public int detectionRadius = 0;

    protected override void Awake() {
        base.Awake();
        intTransform = this.GetComponent<IntTransform>();
    }

    protected IntVector2 GetTargetPos() {
        int bestDistance = int.MaxValue; 
        IntVector2 targetPos = intTransform.GetPos();
        for (int y = -detectionRadius; y <= detectionRadius; y++) {
            for (int x = -detectionRadius; x <= detectionRadius; x++) {
                IntVector2 testPos = intTransform.GetPos() + new IntVector2(x, y);
                GameObject occupant = intTransform.GetLevel().GetOccupantAt(testPos);
                if (occupant == null)
                    continue;
                if (occupant.name != "Player")
                    continue;

                int testDistance = IntVector2.ManDist(intTransform.GetPos(), testPos);
                if (testDistance < bestDistance) {
                    bestDistance = testDistance;
                    targetPos = testPos;
                }
            }
        }
        return targetPos;
    }
}
