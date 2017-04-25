using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct KeyDirectionPair {
    public KeyCode key;
    public IntVector2 direction;
}

public class PlayerController : Controller {
    public List<KeyDirectionPair> keyDirectionPairs;
	
	// Update is called once per frame
	void Update () {
        foreach (KeyDirectionPair keyDirectionPair in keyDirectionPairs) {
            if (Input.GetKeyDown(keyDirectionPair.key)) {
                DoAction(0, keyDirectionPair.direction);
            }
        }
	}
}
