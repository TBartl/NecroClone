using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Weapon))]
[CanEditMultipleObjects]
public class WeaponEditor : ItemEditor {
}
#endif

[CreateAssetMenu(menuName="Items/Weapon") ]
public class Weapon : Item {
    public int damage = 1;
    public float recoverTime = .5f;
	public HitPattern pattern;

	public override void OnEquip(GameObject owner) {
		base.OnEquip(owner);
		ActionHit actionHit = owner.AddComponent<ActionHit>();
		actionHit.damage = this.damage;
		actionHit.recoverTime = this.recoverTime;
		actionHit.pattern = this.pattern;
	}

	public override void OnUnequip(GameObject owner) {
		base.OnEquip(owner);
		Destroy(owner.GetComponent<ActionHit>());
	}
}
