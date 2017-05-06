using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    IntTransform intTransform;

    public int damage;

    void Awake() {
        intTransform = this.GetComponent<IntTransform>();
    }

    public bool CanHit(IntVector2 direction) {
        return true;
    }

    public bool Hit(IntVector2 direction) {
        if (!CanHit(direction))
            return true;
        return false;
    }
}
