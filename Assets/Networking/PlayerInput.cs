using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerInputKey : byte {
    space,
    up, right, down, left,
    num1, num2, num3, num4, num5,
} 

[System.Serializable]
public struct KeyPlayerInputPair {
    public PlayerInputKey playerInput;
    public List<KeyCode> keys;
}

public class PlayerInput : MonoBehaviour {

    public List<KeyPlayerInputPair> pairs;

    void Update() {
        if (!NetManager.S.isConnected)
            return;

        foreach (KeyPlayerInputPair pair in pairs) {
            foreach (KeyCode code in pair.keys) {
                if (Input.GetKeyDown(code)) {
                    NetManager.S.SendClientMessage(new NetMessage_ClientInput(pair.playerInput));
                    break;
                }
            }
        }
    }
}
