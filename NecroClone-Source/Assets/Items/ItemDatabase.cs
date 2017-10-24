using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponID : byte {
    none,

    baseDagger, titaniumDagger, jeweledDagger,
    baseLongsword, titaniumLongsword,
    baseBroadsword, titaniumBroadsword,

    endPlayerWeapons,

    enemySluggish,

    count
}

public class ItemDatabase : MonoBehaviour {

    public static ItemDatabase S;

    Weapon[] weapons = new Weapon[(byte)WeaponID.count];

    void Awake() {
		if (S != null)
			return;
		S = this;

        Weapon[] unordered = Resources.LoadAll<Weapon>("Items");
        foreach (Weapon g in unordered) {
            byte id = (byte)g.id;
            weapons[id] = g;
        }
    }

    public Weapon GetWeapon(WeaponID weaponId) {
        return weapons[(byte)weaponId];
    }
}
