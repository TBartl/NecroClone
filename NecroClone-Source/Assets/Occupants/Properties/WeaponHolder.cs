using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WeaponHolder : MonoBehaviour, IOnMove {

	IntTransform intTransform;
    public Weapon weapon;

	void Awake() {
		intTransform = this.GetComponent<IntTransform>();
		if (weapon)
			weapon.OnEquip(this.gameObject);
	}

	public void OnMove(IntVector2 to) {
		foreach (GameObject collectable in intTransform.GetLevel().GetCollectablesAt(to)) {
			WeaponCollectable weaponCollectable = collectable.GetComponent<WeaponCollectable>();
			if (weaponCollectable) {
				weapon.OnUnequip(this.gameObject);

				weapon = weaponCollectable.weapon;
				weapon.OnEquip(this.gameObject);
			}
		}
	}
}
