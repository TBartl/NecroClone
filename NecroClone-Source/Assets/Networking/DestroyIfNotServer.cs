using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!NetManager.S.isServer)
            Destroy(this.gameObject);
	}
}
