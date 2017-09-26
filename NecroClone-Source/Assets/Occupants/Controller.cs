using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    bool recovering = false;
    protected IntTransform intTransform;

    protected virtual void Awake() {
        intTransform = this.GetComponent<IntTransform>();
    }
    void Start() {
        if (NetManager.S.isServer)
            StartCoroutine(Recover(.5f));
    }

    IEnumerator Recover(float recoveryTime) {
        recovering = true;
        EffectDatabase.S.CreateRecovery(this.transform, recoveryTime);
        yield return new WaitForSeconds(recoveryTime);
        recovering = false;
        if (NetManager.S.isServer)
            OnRecoverFinished();
    }

    protected virtual void OnRecoverFinished() {

    }

    protected virtual bool IsRecovering() {
        return recovering;
    }

    protected void DoAction(Action action, IntVector2 direction) {
        if (recovering)
            Debug.LogError("Did an action while recovering from another one " + action.GetType().ToString());
        else if (!action)
            Debug.LogError("Tried to do an action that doesn't exist!");
        else
            action.RpcExecute(direction);
    }

    //warning: the controller should never use this
    public void DoActionReal(int action, IntVector2 direction) {
        Action[] actions = this.GetComponents<Action>();
        actions[action].Execute(direction);
        StartCoroutine(Recover(actions[action].GetRecoverTime()));
    }
}
