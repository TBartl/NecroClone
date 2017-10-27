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
		List<GameObject> collectables = intTransform.GetLevel().GetCollectablesAt(to);
		foreach (GameObject collectable in collectables.ToArray()) {
			ItemCollectable itemCollectable = collectable.GetComponent<ItemCollectable>();
			if (itemCollectable && itemCollectable.item.GetType() == typeof(Weapon)) {

				// Drop our item
				if (weapon) {
					weapon.OnUnequip(this.gameObject);
					intTransform.GetLevel().SpawnItem(weapon, to);
				}

				// Grab the item
				weapon = (Weapon)itemCollectable.item;
				weapon.OnEquip(this.gameObject);
				Destroy(collectable);
				intTransform.GetLevel().RemoveCollectableAt(collectable, to);

				SoundManager.S.Play(SoundManager.S.pickup);
			}
		}
	}
}
