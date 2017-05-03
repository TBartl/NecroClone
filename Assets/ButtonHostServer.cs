using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHostServer : MonoBehaviour {

    public void OnClick() {
        NetManager.S.StartHost();
    }
}
