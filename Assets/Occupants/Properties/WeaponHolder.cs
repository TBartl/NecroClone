using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WeaponHolder : MonoBehaviour, ChangeableProperty {

    public Weapon weapon;

    public void Encode(ref BinaryWriter writer) {
        writer.Write((byte)weapon.id);
    }
    public void Decode(ref BinaryReader reader) {
        weapon = ItemDatabase.S.GetWeapon((WeaponID)reader.ReadByte());
    }
}
