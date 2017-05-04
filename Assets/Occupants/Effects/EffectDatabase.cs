using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDatabase : MonoBehaviour {
    public static EffectDatabase S;

    public GameObject recover;

    void Awake() {
        S = this;
    }

    public void CreateRecovery(Transform t, float recoverTime) {
        GameObject g = GameObject.Instantiate(recover, t);
        g.transform.localPosition = Vector3.zero;
        g.GetComponent<RecoverEffect>().length = recoverTime;
    }

}
