using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour {
    Camera main;
    public float strength = 2f;
	// Use this for initialization
	void Start () {
        main = Camera.main;
        PlayerIdentity playerIdentity = GetComponentInParent<PlayerIdentity>();
        if (!playerIdentity.IsMyPlayer()) {
            this.enabled = false;
            return;
        }
        main.transform.position = this.transform.position;
        main.transform.rotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        main.transform.position = Vector3.Lerp(main.transform.position, this.transform.position, strength * Time.deltaTime);
        main.transform.rotation = Quaternion.Lerp(main.transform.rotation, this.transform.rotation, strength * Time.deltaTime);
    }
}
