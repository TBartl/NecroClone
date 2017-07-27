﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToDirection : MonoBehaviour {

    static float pitchAmount = 20f;
    static float slideAmount = -.15f;

    void Start() {
        Rotate(IntVector2.down);
        this.transform.localPosition += new Vector3(0, 0, slideAmount);
    }

    public void Rotate(IntVector2 direction) {
        Quaternion rotation = Quaternion.Euler(0, Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg + 90, 0);
        //rotation = Quaternion.Euler(pitchAmount, 0, 0) * rotation;
        this.transform.rotation = rotation;
    }
}
