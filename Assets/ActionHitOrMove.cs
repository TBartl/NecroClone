using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHitOrMove : Action {
    public override void Execute(IntVector2 direction) {
        IntVector2 targetPos = pos.GetPos() + direction;
        GameObject target = LevelManager.S.level.GetOccupantAt(targetPos);
        if (target) {
            pos.Bump(targetPos);
            Destroy(target);
        }else {
            pos.TryMove(pos.GetPos() + direction);
        }
    }
}
