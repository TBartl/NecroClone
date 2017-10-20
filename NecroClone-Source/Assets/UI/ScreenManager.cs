using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public static ScreenManager S;

    GameObject currentScreen;

    public GameObject titleScreen;
    public GameObject lobbyScreen;

    void Awake() {
        S = this;   
    }

    void Start() {
		currentScreen = (GameObject)Instantiate(titleScreen);
    }

}
