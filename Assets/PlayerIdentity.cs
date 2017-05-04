using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour {

    bool isMyPlayer = false;

    public void SetAsMyPlayer() {
        isMyPlayer = true;
    }

    public bool IsMyPlayer() {
        return isMyPlayer;
    }

}
