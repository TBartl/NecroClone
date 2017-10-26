using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
	public Sprite image;

	public virtual void OnEquip(GameObject owner) {
	}

	public virtual void OnUnequip(GameObject owner) {
	}
}
