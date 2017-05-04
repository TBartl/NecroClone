using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFixedRecoverTime : Action {

    public float recoverTime;

    public override float GetRecoverTime() {
        return recoverTime;
    }
}
