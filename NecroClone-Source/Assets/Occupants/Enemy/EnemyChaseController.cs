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
        CheckAndUpdateIfInLineWithPlayer(target);

        IntVector2 currentPos = intTransform.GetPos();
        IntVector2 targetPos = target.GetComponent<IntTransform>().GetPos();
        IntVector2 direction = GetDirection(currentPos, targetPos);
        // If we don't have a shovel, this shouldn't be able to dig, instead change direction
        if (hitDigOrMove.WillBump(direction)) {
            headingHorizontal = !headingHorizontal;
            direction = GetDirection(currentPos, targetPos);
        }
        DoAction(hitDigOrMove.GetAction(direction), direction);

		if (target)
			CheckAndUpdateIfInLineWithPlayer(target);
    }

    public override void OnPlayerMoved(PlayerController player) {
        base.OnPlayerMoved(player);
        CheckAndUpdateIfInLineWithPlayer(player.gameObject);
    }

    void CheckAndUpdateIfInLineWithPlayer(GameObject player) {
        // If the player passes next to the object, make sure the enemy targets where the player was
        IntVector2 playerPos = player.GetComponent<IntTransform>().GetPos();
        IntVector2 thisPos = intTransform.GetPos();
        if (thisPos.x == playerPos.x)
            headingHorizontal = false;
        if (thisPos.y == playerPos.y)
            headingHorizontal = true;
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
