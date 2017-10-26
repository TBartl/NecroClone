using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager S;

	public LevelGenerator generator;

	[HideInInspector] public Level startLevel;
	[HideInInspector] public List<Level> levels;

	void Awake() {
		S = this;
		Level newLevel = CreateNewLevel();
		generator.GetLevel(ref newLevel);
		newLevel.Draw();
		startLevel = newLevel;
	}

	public Level CreateNewLevel() {
		GameObject newLevelGO = new GameObject("Level " + levels.Count.ToString());
		Level newLevel = newLevelGO.AddComponent<Level>();
		newLevel.levelNum = levels.Count;
		levels.Add(newLevel);
		return newLevel;
	}

	public Level GetLevel(int i) {
		return levels[i];
	}



}
