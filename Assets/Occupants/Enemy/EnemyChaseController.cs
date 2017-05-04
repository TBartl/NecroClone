using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseController : EnemyController {
    bool headingHorizontal = true;

    protected override void OnRecoverFinished() {
        IntVector2 currentPos = intTransform.GetPos();
        IntVector2 target = GetTargetPos();
        if (target == intTransform.GetPos())
            DoAction(0, IntVector2.zero);
        else
            DoAction(1, GetDirection(currentPos, target));
    }


    protected IntVector2 GetDirection (IntVector2 from, IntVector2 target) {
        IntVector2 direction = IntVector2.zero;

        if (headingHorizontal) {
            if (from.x != target.x) {
                if (from.x > target.x)
                    direction = IntVector2.left;
                else
                    direction = IntVector2.right;
            }
            else {
                if (from.y > target.y)
                    direction = IntVector2.down;
                else
                    direction = IntVector2.up;
                headingHorizontal = false;
            }
        }
        else {
            if (from.y != target.y) {
                if (from.y > target.y)
                    direction = IntVector2.down;
                else
                    direction = IntVector2.up;
            }
            else {
                if (from.x > target.x)
                    direction = IntVector2.left;
                else
                    direction = IntVector2.right;
                headingHorizontal = true;
            }
        }
        return direction;
    }
}
