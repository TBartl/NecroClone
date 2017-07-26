using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseController : EnemyController {
    bool headingHorizontal = true;
    
    ActionAutoHitDigOrMove hitDigOrMove;

    protected override void Awake() {
        base.Awake();
        hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
    }

    protected override void OnReadyForNextAction(GameObject target) {
        IntVector2 currentPos = intTransform.GetPos();
        IntVector2 targetPos = target.GetComponent<IntTransform>().GetPos();
        IntVector2 direction = GetDirection(currentPos, targetPos);
        // If we don't have a shovel, this shouldn't be able to dig, instead change
        if (this.GetComponent<ActionDig>() == null) {
            GameObject targetGO = intTransform.GetLevel().GetOccupantAt(currentPos + direction);
            if (targetGO != null && targetGO.GetComponent<Diggable>() != null) {
                headingHorizontal = !headingHorizontal;
                direction = GetDirection(currentPos, targetPos);
            }

        }
        DoAction(hitDigOrMove.GetAction(direction), direction);
    }


    protected IntVector2 GetDirection(IntVector2 from, IntVector2 target) {
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
