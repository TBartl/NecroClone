using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile {
	public GameObject floor;
	public GameObject occupant;
	List<GameObject> collectables;

	public void AddCollectable(GameObject collectable) {
		if (collectables == null)
			collectables = new List<GameObject>();
		collectables.Add(collectable);
	}
	public void RemoveCollectable(GameObject collectable) {
		if (collectables == null)
			collectables = new List<GameObject>();
		collectables.Remove(collectable);
	}
	public List<GameObject> GetCollectables() {
		if (collectables == null)
			collectables = new List<GameObject>();
		return collectables;
	}

	public void Draw(IntVector2 position, Level level) {
		if (floor != null) {
			string nameWithoutClone = floor.name;
			floor = (GameObject)GameObject.Instantiate(floor, (Vector3)position, Quaternion.identity, level.transform);
			floor.name = nameWithoutClone;
		}
		if (occupant != null) {
			occupant = level.SpawnOccupant(occupant, position, true);
		}
		if (collectables != null) {
			List<GameObject> newCollectables = new List<GameObject>();
			foreach (GameObject collectable in collectables) {
				newCollectables.Add(level.SpawnCollectable(collectable, position));
			}
			collectables = newCollectables;
		}
	}
}
