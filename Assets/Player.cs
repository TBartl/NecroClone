using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public static Player S;

    public override void OnStartClient() {
        S = this;
    }

    [Command]
    public void CmdUpdateBoard(IntVector2 pos, int command, IntVector2 direction) {
        GameObject occupant = LevelManager.S.level.tiles[pos.x, pos.y].occupant;
        if (occupant == null)
            Debug.LogError("Error! No occupant at point");

        Controller controller = occupant.GetComponent<Controller>();
        if (controller == null)
            Debug.LogError("Error! No controller at point");
        controller.actions[command].Execute(direction);
    }

}
