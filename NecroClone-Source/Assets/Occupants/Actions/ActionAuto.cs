using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAuto<T> where T : Action {

    GameObject owner;
    T action;

    public ActionAuto(GameObject owner) {
        this.owner = owner;
    }

    public bool Exists() {
        return UpdateActionIfNeeded();
    }

    public T GetAction() {
        UpdateActionIfNeeded();
        return action;
    }

    bool UpdateActionIfNeeded() {
        if (action)
            return true;
        action = owner.GetComponent<T>();
        return (action);
    }
}
