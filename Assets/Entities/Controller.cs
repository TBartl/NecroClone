using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public List<Action> actions;
    protected Movable movable;

    protected virtual void Awake() {
        movable = this.GetComponent<Movable>();
    }

    protected void DoAction(int action) {
        DoAction(action, IntVector2.zero);
    }

    protected void DoAction(int action, IntVector2 direction) {
        Player.S.CmdUpdateBoard(movable.GetPos(), action, direction);
    }

}
