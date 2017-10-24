using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetManager.S.ResetNetwork();
	}
}
