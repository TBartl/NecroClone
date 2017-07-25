using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHit : ActionFixedRecoverTime {
    public int damage;

    List<GameObject> validTargets;

    public virtual bool CanHit(IntVector2 direction) {
        return GetValidTargets(direction).Count > 0;
    }

    public override void Execute(IntVector2 direction) {
        List<GameObject> validTargets = GetValidTargets(direction);
        foreach (GameObject target in validTargets) {
            target.GetComponent<Killable>().Hit(damage);
        }

        intTransform.Bump(intTransform.GetPos() + direction);
        SoundManager.S.Play(SoundManager.S.hit);
    }

    protected virtual List<GameObject> GetValidTargets(IntVector2 direction) {
        validTargets = new List<GameObject>();

        IntVector2 targetPos = intTransform.GetPos() + direction;


        //Dagger
        AddIfValidTarget(targetPos);

        //if (weaponType == WeaponType.longsword) {
        //    target = intTransform.GetLevel().GetOccupantAt(targetPos + direction);
        //    if (IsValidTarget(target))
        //        validTargets.Add(target);
        //}

        //if (weaponType == WeaponType.broadsword) {
        //    if (direction == IntVector2.up || direction == IntVector2.down) {
        //        target = intTransform.GetLevel().GetOccupantAt(targetPos + IntVector2.left);
        //        if (IsValidTarget(target))
        //            validTargets.Add(target);
        //        target = intTransform.GetLevel().GetOccupantAt(targetPos + IntVector2.right);
        //        if (IsValidTarget(target))
        //            validTargets.Add(target);
        //    }
        //    else if (direction == IntVector2.left || direction == IntVector2.right) {
        //        target = intTransform.GetLevel().GetOccupantAt(targetPos + IntVector2.up);
        //        if (IsValidTarget(target))
        //            validTargets.Add(target);
        //        target = intTransform.GetLevel().GetOccupantAt(targetPos + IntVector2.down);
        //        if (IsValidTarget(target))
        //            validTargets.Add(target);
        //    }
        //}

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
        if (target.GetComponent<Killable>() == null)
            return false;
        return true;
    }
}
