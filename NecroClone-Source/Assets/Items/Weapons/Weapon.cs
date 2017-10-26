using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType {
    dagger,
    spear,
    broadsword,
	rapier,
}

[CreateAssetMenu(menuName="Items/Weapon") ]
public class Weapon : Item {
    public WeaponType weaponType;
    public int damage = 1;
    public float recoverTime = .5f;

	public override void OnEquip(GameObject owner) {
		base.OnEquip(owner);
		ActionHit actionHit = owner.AddComponent<ActionHit>();
		actionHit.damage = this.damage;
		actionHit.recoverTime = this.recoverTime;
	}

	public override void OnUnequip(GameObject owner) {
		base.OnEquip(owner);
		Destroy(owner.GetComponent<ActionHit>());
	}
}
