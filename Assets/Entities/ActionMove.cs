using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action {

    public override void Execute(IntVector2 direction) {
        pos.TryMove(pos.GetPos() + direction);
    }
}
