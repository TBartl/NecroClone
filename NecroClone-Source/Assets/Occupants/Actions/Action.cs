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

    public void RpcExecute(IntVector2 direction) {
        Action[] actions = this.GetComponents<Action>();
        for (int i = 0; i < actions.Length; i++) {
            Action action = actions[i];
            if (action == this) {
                NetManager.S.SendServerMessageToGroup(new NetMessage_ActionOccupant(intTransform.GetPos(), intTransform.GetLevel(), i, direction), ConnectionGroup.game);
                return;
            }

        }
        Debug.LogError("Tried to do an action that didn't exist " + this.GetType().ToString());
    }

    public virtual void Execute(IntVector2 direction) {

    }


}
