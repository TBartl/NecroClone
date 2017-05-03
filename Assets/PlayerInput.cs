using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerInputKey : byte {
    space,
    up, right, down, left
} 

public class PlayerInput : MonoBehaviour {

    void Update() {
        if (!NetManager.S.isConnected)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            NetManager.S.SendClientMessage(new NetMessage_ClientInput(PlayerInputKey.space));

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            NetManager.S.SendClientMessage(new NetMessage_ClientInput(PlayerInputKey.up));

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            NetManager.S.SendClientMessage(new NetMessage_ClientInput(PlayerInputKey.right));

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            NetManager.S.SendClientMessage(new NetMessage_ClientInput(PlayerInputKey.down));

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            NetManager.S.SendClientMessage(new NetMessage_ClientInput(PlayerInputKey.left));


    }

}
