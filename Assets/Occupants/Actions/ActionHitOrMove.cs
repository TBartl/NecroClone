﻿using System.Collections;
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
        IntVector2 targetPos = intTransform.GetPos() + direction;
        GameObject target = intTransform.GetLevel().GetOccupantAt(targetPos);
        if (target) {
            intTransform.Bump(targetPos);
            recoverTime = hitRecoverTime;

            Killable otherKillable = target.GetComponent<Killable>();
            if (otherKillable) {
                SoundManager.S.Play(SoundManager.S.hit);
            }

        }else {
            intTransform.TryMove(intTransform.GetPos() + direction);
            recoverTime = moveRecoverTime;
        }
    }
}