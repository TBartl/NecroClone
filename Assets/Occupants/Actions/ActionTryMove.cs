using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTryMove : ActionFixedRecoverTime {

    public override void Execute(IntVector2 direction) {
        intTransform.TryMove(intTransform.GetPos() + direction);
    }

    public bool CanMove(IntVector2 direction) {
        return intTransform.CanMove(intTransform.GetPos() + direction);
    }
}
