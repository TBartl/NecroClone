using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHitOrMove : Action {
    public float moveRecoverTime;
    public float hitRecoverTime;
    float recoverTime;

    public override float GetRecoverTime() {
        return recoverTime;
    }

    public override void Execute(IntVector2 direction) {
        IntVector2 targetPos = pos.GetPos() + direction;
        GameObject target = LevelManager.S.level.GetOccupantAt(targetPos);
        if (target) {
            pos.Bump(targetPos);
            recoverTime = hitRecoverTime;
            //if (target.CompareTag("Indestructable") == false)
            Destroy(target);
            SoundManager.S.Play(SoundManager.S.hit);
        }else {
            pos.TryMove(pos.GetPos() + direction);
            recoverTime = moveRecoverTime;
        }
    }
}
