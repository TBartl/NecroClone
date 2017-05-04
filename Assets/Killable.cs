using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : Destructable {
    public int maxHealth = 1;
    int health;

    void Awake() {
        health = maxHealth;
    }

    public void Hit(int damage) {
        health -= damage;
        if (health <= 0) {
            this.GetComponent<Destructable>().DestroyThis();
        }
    }
}
