using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager S;

    [HideInInspector] public Level level;
    public LevelGenerator levelGenerator;

    void Awake() {
        S = this;
    }

    void Start() {
        level = levelGenerator.GetLevel();
        level.Draw();
    }
}
