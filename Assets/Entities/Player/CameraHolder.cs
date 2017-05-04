using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour {
    Camera main;
	// Use this for initialization
	void Start () {
        main = Camera.main;
        PlayerIdentity playerIdentity = GetComponentInParent<PlayerIdentity>();
        if (!playerIdentity.IsMyPlayer()) {
            this.enabled = false;
            return;
        }
        MoveCameraHere();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        MoveCameraHere();
    }

    void MoveCameraHere() {
        main.transform.position = this.transform.position;
        main.transform.rotation = this.transform.rotation;
    }
}
