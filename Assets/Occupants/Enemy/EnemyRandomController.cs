using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomController : EnemyController {

    IntVector2[] randomDirections = { IntVector2.up, IntVector2.right, IntVector2.down, IntVector2.left };

    ActionFixedRecoverTime actionFixedRecoverTime;
    ActionHitDigOrMove actionHitDigOrMove;

    protected override void Awake() {
        base.Awake();
        actionFixedRecoverTime = this.GetComponent<ActionFixedRecoverTime>();
        actionHitDigOrMove = this.GetComponent<ActionHitDigOrMove>();
    }

    protected override void OnRecoverFinished() {
        IntVector2 target = GetTargetPos();
        if (target == intTransform.GetPos())
            DoAction(actionFixedRecoverTime, IntVector2.zero);
        else {
            IntVector2 direction = randomDirections[Random.Range(0, randomDirections.Length)];
            DoAction(actionHitDigOrMove, direction);
        }
    }
}
