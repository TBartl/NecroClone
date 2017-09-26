using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Killable : Destructable, ChangeableProperty {
    public int maxHealth = 1;
    int health;

    Transform heartHolder;
    float heartSeparation = .15f;
    List<GameObject> hearts = new List<GameObject>();

    void Awake() {
        health = maxHealth;
        heartHolder = this.transform;
        SmoothMove smoothMove = GetComponentInChildren<SmoothMove>();
        if (smoothMove)
            heartHolder = smoothMove.transform;

        UpdateHearts();
    }

    public void Encode(ref BinaryWriter writer) {
        writer.Write(maxHealth);
        writer.Write(health);
    }
    public void Decode(ref BinaryReader reader) {
        maxHealth = reader.ReadInt32();
        health = reader.ReadInt32();
        UpdateHearts();
    }

    public void Hit(int damage) {
        health -= damage;
        if (health <= 0) {
            DestroyThis();
        }
        else {
            UpdateHearts();
        }
    }

    void UpdateHearts() {
        while (hearts.Count != health) {
            if (hearts.Count > health) {
                Destroy(hearts[hearts.Count - 1]);
                hearts.RemoveAt(hearts.Count - 1);
            }
            else {
                GameObject newHeart = (GameObject)Instantiate(EffectDatabase.S.heart);
                newHeart.transform.parent = heartHolder;
                newHeart.transform.localPosition = Vector3.zero;
                hearts.Add(newHeart);
            }
        }

        for (int i = 0; i < hearts.Count; i++) {
            hearts[i].transform.localPosition = Vector3.right * (- hearts.Count / 2f + i + .5f) * heartSeparation;
        }
    }
}
