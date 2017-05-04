using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

    protected IntVectorPos pos;
    public float recoverTime = .5f;

    protected virtual void Awake() {
        pos = this.GetComponent<IntVectorPos>();
    }
    
    public void RpcExecute(IntVector2 direction, int actionIndex) {
        NetManager.S.SendServerMessageToAll(new NetMessage_ActionOccupant(pos.GetPos(), actionIndex, direction));
    }

    public virtual void Execute(IntVector2 direction) {

    }


}
