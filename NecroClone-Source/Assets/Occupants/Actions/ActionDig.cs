using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDig : ActionFixedRecoverTime {
    //public int shovelStrength;

    List<GameObject> validTargets;

    public virtual bool CanDig(IntVector2 direction) {
        return GetValidTargets(direction).Count > 0;
    }

    public override void Execute(IntVector2 direction) {
        List<GameObject> validTargets = GetValidTargets(direction);
        foreach (GameObject target in validTargets) {
            target.GetComponent<Diggable>().Dig(/*shovelStrength*/);
        }
        foreach (SmoothMove smoothMove in this.GetComponentsInChildren<SmoothMove>())
            smoothMove.Bump(intTransform.GetPos() + direction);
        SoundManager.S.Play(SoundManager.S.hit);
    }

    protected virtual List<GameObject> GetValidTargets(IntVector2 direction) {
        validTargets = new List<GameObject>();

        IntVector2 targetPos = intTransform.GetPos() + direction;

        AddIfValidTarget(targetPos);

        return validTargets;
    }

    bool AddIfValidTarget(IntVector2 targetPos) {
        GameObject target = intTransform.GetLevel().GetOccupantAt(targetPos);
        if (IsValidTarget(target)) {
            validTargets.Add(target);
            return true;
        }
        return false;
    }

    bool IsValidTarget(GameObject target) {
        if (target == null)
            return false;
        if (target.GetComponent<Diggable>() == null)
            return false;
        return true;
    }
}
