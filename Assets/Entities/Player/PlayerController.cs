using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct KeyDirectionPair {
    public KeyCode key;
    public IntVector2 direction;
}

public class PlayerController : NetworkBehaviour {
    
    public List<KeyDirectionPair> keyDirectionPairs;
    public Action move;
    //List<KeyCode> buffer = new List<KeyCode>(); TODO

    [Client]
    void Update() {
        foreach (KeyDirectionPair keyDirectionPair in keyDirectionPairs) {
            if (Input.GetKeyDown(keyDirectionPair.key)) {
                CmdSendInput(keyDirectionPair.key);
            }
        }
    }

    [Command]
    public void CmdSendInput(KeyCode input) {
        foreach (KeyDirectionPair keyDirectionPair in keyDirectionPairs) {
            if (input == keyDirectionPair.key) {
                move.RpcExecute(keyDirectionPair.direction);
                break;
            }
        }
    }
}
