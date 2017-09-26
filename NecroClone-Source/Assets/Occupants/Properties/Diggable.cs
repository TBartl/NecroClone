using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diggable : Destructable {

    public void Dig() {
        DestroyThis();
    }
}
