using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "LevelGen/levelBuilder")]
public class LevelGeneratorFromLevelBuilder : LevelGenerator {

	public override void GetLevel(ref Level level) {
		GameObject rootGO = GameObject.Find("LevelBuilder");
		if (!rootGO) {
			Debug.LogError("Could not find Level Builder!");
			return;
		}

		Transform root = rootGO.transform;
		IntVector2 boundsMin = new IntVector2(int.MaxValue, int.MaxValue);
		IntVector2 boundsMax = new IntVector2(int.MinValue, int.MinValue);
		foreach (Transform child in root) {
			IntVector2 childPos = IntVector2.fromVector3(child.transform.position);
			boundsMin.x = Mathf.Min(boundsMin.x, childPos.x);
			boundsMin.y = Mathf.Min(boundsMin.y, childPos.y);
			boundsMax.x = Mathf.Max(boundsMax.x, childPos.x);
			boundsMax.y = Mathf.Max(boundsMax.y, childPos.y);
		}
		IntVector2 size = boundsMax - boundsMin + IntVector2.one;
		level.Resize(size);

		foreach (Transform child in root) {
			IntVector2 childPos = IntVector2.fromVector3(child.transform.position);
			IntVector2 adjustedPos = childPos - boundsMin;
			string correctedName = child.name.Split(' ')[0];

			if (correctedName == "Spawn") {
				level.spawnPositions.Add(adjustedPos);
				continue;
			}

			GameObject floorPrefab = LevelDatabase.S.GetFloorPrefab(correctedName);
			if (floorPrefab) {
				level.tiles[adjustedPos.x, adjustedPos.y].floor = floorPrefab;
				continue;
			}

			GameObject occupantPrefab = LevelDatabase.S.GetOccupantPrefab(correctedName);
			if (occupantPrefab) {
				level.tiles[adjustedPos.x, adjustedPos.y].occupant = occupantPrefab;
				continue;
			}
			GameObject collectablePrefab = LevelDatabase.S.GetCollectablePrefab(correctedName);
			if (collectablePrefab) {
				level.tiles[adjustedPos.x, adjustedPos.y].AddCollectable(collectablePrefab);
				continue;
			}
		}
		Destroy(rootGO);
	}

}
