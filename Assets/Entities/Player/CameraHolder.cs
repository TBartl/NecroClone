using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour {
    Camera main;
	// Use this for initialization
	void Start () {
        main = Camera.main;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        main.transform.position = this.transform.position;
        main.transform.rotation = this.transform.rotation;

    }
}
