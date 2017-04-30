using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "LevelGen/Base")]
public class LevelGenerator : ScriptableObject {

    public virtual void GenLevel(Level level) {
    }

}
