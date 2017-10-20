using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPField : MonoBehaviour {

    InputField field;

    // Use this for initialization
    void Start() {
        field = this.GetComponent<InputField>();
        field.text = SaveManager.S.saveData.ip;
    }

    public void OnFieldChanged() {
        SaveManager.S.saveData.ip = field.text;
        SaveManager.S.Save();
    }
}
