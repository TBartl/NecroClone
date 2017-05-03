using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KeyDirectionPair {
    public KeyCode key;
    public IntVector2 direction;
}

public class PlayerController : MonoBehaviour {
    
    public List<KeyDirectionPair> keyDirectionPairs;
    public Action move;
    //List<KeyCode> buffer = new List<KeyCode>(); TODO
    
    void Update() {
        foreach (KeyDirectionPair keyDirectionPair in keyDirectionPairs) {
            if (Input.GetKeyDown(keyDirectionPair.key)) {
                CmdSendInput(keyDirectionPair.key);
            }
        }
    }

    public void CmdSendInput(KeyCode input) {
        foreach (KeyDirectionPair keyDirectionPair in keyDirectionPairs) {
            if (input == keyDirectionPair.key) {
                move.RpcExecute(keyDirectionPair.direction);
                break;
            }
        }
    }
}
