using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHover : MonoBehaviour {
	public float amount = .1f;
	public float speed = 5;

	float r;

	void Start() {
		r = Random.value * 10f;
	}

	// Update is called once per frame
	void Update () {
		this.transform.localPosition = Vector3.up * Mathf.Sin(Time.time * speed + r) * amount;		
	}
}
