using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameField : MonoBehaviour {

    InputField field;

	// Use this for initialization
	void Start () {
        field = this.GetComponent<InputField>();
        field.text = SaveManager.S.saveData.name;
	}

    public void OnFieldChanged() {
        SaveManager.S.saveData.name = field.text;
        SaveManager.S.Save();
    }
}
