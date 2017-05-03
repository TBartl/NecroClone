using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConnectServer : MonoBehaviour {

    public Text inputField;

    public void OnClick() {
        NetManager.S.StartConnect(inputField.text);
    }
}
