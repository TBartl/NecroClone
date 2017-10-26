using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WeaponHolder : MonoBehaviour {

    public Weapon weapon;

	void Awake() {
		if (weapon)
			weapon.OnEquip(this.gameObject);
	}
}
