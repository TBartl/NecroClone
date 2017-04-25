using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Actions - Instant things that affect the board state
// Move (Direction)
// Attack (Direction)
// Spell


public class Action : MonoBehaviour {

    public virtual void Execute() {

    }

    public virtual void Execute(IntVector2 direction) {
        Execute();
    }
}
