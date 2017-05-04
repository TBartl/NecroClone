using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {
    protected IntVectorPos intTransform;
    public int detectionRadius = 0;

    protected override void Awake() {
        base.Awake();
        intTransform = this.GetComponent<IntVectorPos>();
    }

    protected IntVector2 GetTargetPos() {
        int bestDistance = int.MaxValue; 
        IntVector2 targetPos = intTransform.GetPos();
        for (int y = -detectionRadius; y <= detectionRadius; y++) {
            for (int x = -detectionRadius; x <= detectionRadius; x++) {
                IntVector2 testPos = intTransform.GetPos() + new IntVector2(x, y);
                GameObject occupant = LevelManager.S.level.GetOccupantAt(testPos);
                if (occupant == null)
                    continue;
                if (occupant.GetComponent<DatabaseID_Occupant>().GetOccupantID() != OccupantId.player)
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
