using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    Action[] actions;
    bool recovering = false;
    protected IntTransform intTransform;

    protected virtual void Awake() {
        actions = this.GetComponents<Action>();
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

    protected void DoAction(int action, IntVector2 direction) {
        if (recovering)
            Debug.LogError("Did an action while recovering from another one");

        if (action < 0 || action >= actions.Length)
            Debug.LogError("ERROR: Action does not exist!");
        actions[action].RpcExecute(direction, action);
    }

    //warning: the controller should never use this
    public void DoActionReal(int action, IntVector2 direction) {
        actions[action].Execute(direction);
        StartCoroutine(Recover(actions[action].GetRecoverTime()));
    }

    protected int GetActionCount() {
        return actions.Length;
    }
}
