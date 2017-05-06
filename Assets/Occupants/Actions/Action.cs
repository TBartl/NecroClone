using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

    protected IntTransform intTransform;

    public virtual float GetRecoverTime() {
        return 0;
    }

    protected virtual void Awake() {
        intTransform = this.GetComponent<IntTransform>();
    }
    
    public void RpcExecute(IntVector2 direction, int actionIndex) {
        NetManager.S.SendServerMessageToAll(new NetMessage_ActionOccupant(intTransform.GetPos(), intTransform.GetLevel(), actionIndex, direction));
    }

    public virtual void Execute(IntVector2 direction) {

    }


}
