using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConnectServer : MonoBehaviour {
    
    public void OnClick() {
        NetManager.S.StartConnect();
    }
}
