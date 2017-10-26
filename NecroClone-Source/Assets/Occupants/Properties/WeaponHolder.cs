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
			ItemCollectable itemCollectable = collectable.GetComponent<ItemCollectable>();
			if (itemCollectable && itemCollectable.item.GetType() == typeof(Weapon)) {
				weapon.OnUnequip(this.gameObject);

				weapon = (Weapon)itemCollectable.item;
				weapon.OnEquip(this.gameObject);
			}
		}
	}
}
