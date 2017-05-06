using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCycleWeapon : ActionFixedRecoverTime {

    public override void Execute(IntVector2 direction) {
        WeaponHolder weaponHolder = this.GetComponent<WeaponHolder>();
        weaponHolder.weapon = ItemDatabase.S.GetWeapon((WeaponID) (((byte)weaponHolder.weapon.id + 1) % (byte)WeaponID.endPlayerWeapons));
        Debug.LogError("Weapon cycled to: " + weaponHolder.weapon.id.ToString());
    }
}
