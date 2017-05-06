using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Killable : Destructable, ChangeableProperty {
    public int maxHealth = 1;
    int health;

    void Awake() {
        health = maxHealth;
    }

    public void Encode(ref BinaryWriter writer) {
        writer.Write(maxHealth);
        writer.Write(health);
    }
    public void Decode(ref BinaryReader reader) {
        maxHealth = reader.ReadInt32();
        health = reader.ReadInt32();
    }

    public void Hit(int damage) {
        health -= damage;
        if (health <= 0) {
            DestroyThis();
        }
    }
}
