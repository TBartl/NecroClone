using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

    protected IntTransform pos;

    public virtual float GetRecoverTime() {
        return 0;
    }

    protected virtual void Awake() {
        pos = this.GetComponent<IntTransform>();
    }
    
    public void RpcExecute(IntVector2 direction, int actionIndex) {
        NetManager.S.SendServerMessageToAll(new NetMessage_ActionOccupant(pos.GetPos(), actionIndex, direction));
    }

    public virtual void Execute(IntVector2 direction) {

    }


}
