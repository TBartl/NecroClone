using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager {

    public static NetManager S;

    void Awake() {
        if (S == null)
            S = this;
    }
}
