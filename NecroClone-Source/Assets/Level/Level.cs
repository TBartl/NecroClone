using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour {
    public int levelNum;
    public Tile[,] tiles;
    [HideInInspector] public IntVector2 size;
    [HideInInspector] public List<IntVector2> spawnPositions = new List<IntVector2>();
    List<PlayerController> players = new List<PlayerController>();

    public void Resize(IntVector2 size) {
        this.size = size;
        tiles = new Tile[size.x, size.y];
    }

    public void Draw() {
        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                tiles[x, y].Draw(new IntVector2(x, y), this);
            }
        }
    }

    public bool InBounds(IntVector2 pos) {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y);
    }

    public bool Occuppied(IntVector2 pos) {
        if (!InBounds(pos))
            return true;
        return tiles[pos.x, pos.y].occupant != null;
    }

    public IntVector2 GetOpenPlayerSpawnPosition() {
        foreach (IntVector2 pos in ShuffleSpawnPositions()) {
            if (Occuppied(pos) == false) {
                return pos;
            }
        }
        return IntVector2.error;
    }
    List<IntVector2> ShuffleSpawnPositions() {
        List<IntVector2> copy = new List<IntVector2>(spawnPositions);
        for (int i = 0; i < copy.Count; i++) {
            IntVector2 temp = copy[i];
            int randomIndex = Random.Range(i, copy.Count);
            copy[i] = copy[randomIndex];
            copy[randomIndex] = temp;
        }
        return copy;
    }

    public GameObject SpawnOccupant(GameObject prefab, IntVector2 pos, bool overrideOccupied = false) {
        if (prefab == null || (Occuppied(pos) && !overrideOccupied)) {
            Debug.LogError("Error adding occupant!");
            return null;
        }

        GameObject instance = Instantiate(prefab, this.transform);
        instance.name = prefab.name;
        tiles[pos.x, pos.y].occupant = instance;
        instance.transform.position = (Vector3)pos;

        IntTransform mov = instance.GetComponent<IntTransform>();
        if (mov) {
            mov.SetPos(pos);
        }

        PlayerController playerController = instance.GetComponent<PlayerController>();
        if (playerController != null) {
            players.Add(playerController);
        }

        return instance;
    }
	public GameObject SpawnCollectable(GameObject prefab, IntVector2 pos) {
		if (prefab == null) {
			Debug.LogError("Error adding collectable");
			return null;
		}

		GameObject instance = Instantiate(prefab, this.transform);
		instance.name = prefab.name;
		instance.transform.position = (Vector3)pos;

		IntTransform mov = instance.GetComponent<IntTransform>();
		if (mov) {
			mov.SetPos(pos);
		}

		return instance;
	}

    public void OnPlayerAction(PlayerController controller) {
        // TODO check if still on this level
        foreach (EnemyController e in this.GetComponentsInChildren<EnemyController>()) {
            e.OnPlayerMoved(controller);
        }
    }

    public GameObject GetOccupantAt(IntVector2 pos) {
		if (!InBounds(pos))
			return null;
		return tiles[pos.x, pos.y].occupant;
	}
	public List<GameObject> GetCollectablesAt(IntVector2 pos) {
		if (!InBounds(pos))
			return null;
		return tiles[pos.x, pos.y].GetCollectables();
	}
}
