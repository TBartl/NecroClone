using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Actions - Instant things that affect the board state
// Move (Direction)
// Attack (Direction)
// Spell


public class Action : NetworkBehaviour {

    [ClientRpc]
    public void RpcExecute(IntVector2 direction) {
        Execute(direction);
    }

    protected virtual void Execute(IntVector2 direction) {

    }


}
