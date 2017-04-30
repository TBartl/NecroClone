using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action {
    Movable movable;

    protected virtual void Awake() {
        movable = this.GetComponent<Movable>();
    }

    protected override void Execute(IntVector2 direction) {
        movable.TryMove(movable.GetPos() + direction);
    }
}
