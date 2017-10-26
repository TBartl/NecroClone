using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType : byte {
    dagger,
    longsword,
    broadsword,
}

[CreateAssetMenu]
public class Weapon : ScriptableObject {
    public WeaponType weaponType;
    public int damage = 1;
    public float recoverTime = .5f;

	public void OnEquip(GameObject owner) {
		ActionHit actionHit = owner.AddComponent<ActionHit>();
		actionHit.damage = this.damage;
		actionHit.recoverTime = this.recoverTime;
	}
}
