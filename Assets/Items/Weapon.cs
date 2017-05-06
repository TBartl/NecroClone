using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType : byte {
    dagger,
    broadsword,
    spear,
}

[CreateAssetMenu]
public class Weapon : ScriptableObject {

    public WeaponType weaponType;
    public int damage = 1;
    public float recoverTime = .5f;
    
    public virtual bool CanHit(GameObject owner, IntVector2 direction) {
        return GetValidTargets(owner, direction).Count > 0;
    }

    public virtual bool Hit(GameObject owner, IntVector2 direction) {
        List<GameObject> validTargets = GetValidTargets(owner, direction);
        if (validTargets.Count == 0)
            return false;
        foreach(GameObject target in validTargets) {
            target.GetComponent<Killable>().Hit(damage);
        }
        return false;
    }

    List<GameObject> GetValidTargets(GameObject owner, IntVector2 direction) {
        IntTransform intTransform = owner.GetComponent<IntTransform>();
        IntVector2 targetPos = intTransform.GetPos() + direction;

        List<GameObject> validTargets = new List<GameObject>();

        //Dagger
        GameObject target = intTransform.GetLevel().GetOccupantAt(targetPos);
        if (IsValidTarget(target))
            validTargets.Add(target);

        if (weaponType == WeaponType.spear) {
            target = intTransform.GetLevel().GetOccupantAt(targetPos + direction);
            if (IsValidTarget(target))
                validTargets.Add(target);
        }

        if (weaponType == WeaponType.broadsword) {
            if (direction == IntVector2.up || direction == IntVector2.down) {
                target = intTransform.GetLevel().GetOccupantAt(targetPos + direction + IntVector2.left);
                if (IsValidTarget(target))
                    validTargets.Add(target);
                target = intTransform.GetLevel().GetOccupantAt(targetPos + direction + IntVector2.right);
                if (IsValidTarget(target))
                    validTargets.Add(target);
            }
            else if (direction == IntVector2.left || direction == IntVector2.right) {
                target = intTransform.GetLevel().GetOccupantAt(targetPos + direction + IntVector2.up);
                if (IsValidTarget(target))
                    validTargets.Add(target);
                target = intTransform.GetLevel().GetOccupantAt(targetPos + direction + IntVector2.down);
                if (IsValidTarget(target))
                    validTargets.Add(target);
            }
        }

        return validTargets;
    }

    bool IsValidTarget(GameObject target) {
        if (target == null)
            return false;
        if (target.GetComponent<Killable>() == null)
            return false;
        return true;
    }
}
