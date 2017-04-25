using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour {
    static PersistentManager S;

    void Awake() {
        if (S != null) {
            Destroy(this.gameObject);
            return;
        }
        S = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
