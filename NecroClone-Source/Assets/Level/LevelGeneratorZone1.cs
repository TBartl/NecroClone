using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelGen/zone1")]
public class LevelGeneratorZone1 : LevelGenerator {

	public GameObject floor;
	public GameObject wallBase;

	public float enemySpawnRate = .18f;
	public List<GameObject> enemies;

	public float weaponSpawnRate = .005f;
	public List<GameObject> weapons;

	public int smallestRoomSize = 3;
	public int largestRoomSize = 5;
	public int numRooms = 20;

	struct Room {
		public IntVector2 lowerCorner;
		public IntVector2 size;
		public IntVector2 upperCorner {
			get {
				return lowerCorner + size;
			}
		}

		public Room(IntVector2 corner, IntVector2 size) {
			this.lowerCorner = corner;
			this.size = size;
		}
		public static Room RandSized(int minSize, int maxSize) {
			return new Room(IntVector2.zero, new IntVector2(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize)));
		}

		public bool Overlaps(Room other) {
			if (this.lowerCorner.x > other.upperCorner.x || other.lowerCorner.x > this.upperCorner.x)
				return false;
			if (this.lowerCorner.y > other.upperCorner.y || other.lowerCorner.y > this.upperCorner.y)
				return false;
			return true;
		}
	}

	public override void GetLevel(ref Level level) {
		Random.InitState((int)System.DateTime.Now.Ticks);

		List<Room> rooms = new List<Room>();
		rooms.Add(Room.RandSized(smallestRoomSize, largestRoomSize));
		while (rooms.Count < numRooms) {
			Room fromRoom = rooms[Random.Range(0, rooms.Count)];
			Room newRoom = Room.RandSized(smallestRoomSize, largestRoomSize);
			newRoom.lowerCorner = fromRoom.lowerCorner;
			newRoom.lowerCorner += new IntVector2(Random.Range(-largestRoomSize / 2, (largestRoomSize + 1) / 2), Random.Range(-largestRoomSize / 2, (largestRoomSize + 1) / 2));
			rooms.Add(newRoom);
		}

		bool changed = true;
		int iters = 0;
		while (changed) {
			changed = false;
			for (int i = 0; i < rooms.Count; i++) {
				for (int j = 0; j < rooms.Count; j++) {
					if (i == j)
						continue;
					if (rooms[i].Overlaps(rooms[j])) {
						Room modified = rooms[i];
						IntVector2 dir = new IntVector2(
							(int)Mathf.Sign(rooms[i].lowerCorner.x - rooms[j].lowerCorner.x),
							(int)Mathf.Sign(rooms[i].lowerCorner.y - rooms[j].lowerCorner.y));
						int moveStyle = Mathf.FloorToInt(Random.value * 3);
						if (moveStyle == 0)
							dir.x = 0;
						if (moveStyle == 1)
							dir.y = 0;
						modified.lowerCorner += dir;
						rooms[i] = modified;
						changed = true;
					}
				}
			}
			iters++;
			if (iters > 100)
				break;
		}

		// Put rooms into map
		Dictionary<IntVector2, Tile> tiles = new Dictionary<IntVector2, Tile>();
		foreach (Room room in rooms) {
			for (int y = -1; y <= room.size.y+1; y++) {
				for (int x = -1; x <= room.size.x+1; x++) {
					IntVector2 tilePos = new IntVector2(room.lowerCorner.x + x, room.lowerCorner.y + y);
					Tile tile = new Tile();
					tile.floor = floor;
					if (x <= -1 || x >= room.size.x+1 || y <= -1 || y >= room.size.y+1) {
						tile.occupant = wallBase;
					}
					tiles[tilePos] = tile;
				}
			}
		}

		// Remove connection
		List<IntVector2> tileKeys = new List<IntVector2>(tiles.Keys);
		foreach (IntVector2 pos in tileKeys) {
			Tile tile = tiles[pos];
			if (tile.floor != null && tile.occupant != null) {
				if (((OnlyFloorAt(ref tiles, pos + IntVector2.left) && OnlyFloorAt(ref tiles, pos + IntVector2.right)) ||
					(OnlyFloorAt(ref tiles, pos + IntVector2.up) && OnlyFloorAt(ref tiles, pos + IntVector2.down)))) {
					tile.occupant = null;
					tiles[pos] = tile;
				}
			}
		}

		// Add an extra two layer of walls
		for (int i = 0; i < 2; i++) {
			Dictionary<IntVector2, Tile> tilesCopy = new Dictionary<IntVector2, Tile>();
			IntVector2[] dirs = { IntVector2.up, IntVector2.right, IntVector2.down, IntVector2.left };
			foreach (KeyValuePair<IntVector2, Tile> pair in tiles) {
				tilesCopy[pair.Key] = pair.Value;
				foreach (IntVector2 dir in dirs) {
					if (!tiles.ContainsKey(pair.Key + dir)) {
						Tile t = new Tile();
						t.floor = floor;
						t.occupant = wallBase;
						tilesCopy[pair.Key + dir] = t;
					}
				}
			}
			tiles = tilesCopy;
		}

		// Get the size of the map
		IntVector2 bottomLeft = new IntVector2(int.MaxValue, int.MaxValue);
		IntVector2 topRight = new IntVector2(int.MinValue, int.MinValue);
		foreach (KeyValuePair<IntVector2, Tile> pair in tiles) {
			IntVector2 pos = pair.Key;
			bottomLeft.x = Mathf.Min(bottomLeft.x, pos.x);
			bottomLeft.y = Mathf.Min(bottomLeft.y, pos.y);
			topRight.x = Mathf.Max(topRight.x, pos.x);
			topRight.y = Mathf.Max(topRight.y, pos.y);
		}
		IntVector2 size = topRight - bottomLeft + IntVector2.one;

		// Add enemies
		tileKeys = new List<IntVector2>(tiles.Keys);
		foreach (IntVector2 pos in tileKeys) {
			Tile tile = tiles[pos];
			if (tile.occupant == null && Random.value < enemySpawnRate) {
				float spawnIndex = (pos.y - bottomLeft.y) / (float)size.y;
				spawnIndex += Random.Range(-.20f, .20f);
				spawnIndex = Mathf.Clamp(spawnIndex, 0, .999f);
				tile.occupant = enemies[Mathf.FloorToInt(spawnIndex * enemies.Count)];
				tiles[pos] = tile;
			}
		}

		// Add weapons
		tileKeys = new List<IntVector2>(tiles.Keys);
		foreach (IntVector2 pos in tileKeys) {
			Tile tile = tiles[pos];
			if (tile.occupant == null && Random.value < weaponSpawnRate) {
				float spawnIndex = (pos.y - bottomLeft.y) / (float)size.y;
				spawnIndex += Random.Range(-.20f, .20f);
				spawnIndex = Mathf.Clamp(spawnIndex, 0, .999f);
				tile.AddCollectable(weapons[Mathf.FloorToInt(spawnIndex * weapons.Count)]);
				tiles[pos] = tile;
			}
		}

		// Convert the map to an array
		level.Resize(size);
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				IntVector2 realCoord = new IntVector2(x, y) + bottomLeft;
				if (tiles.ContainsKey(realCoord))
					level.tiles[x, y] = tiles[realCoord];
			}
		}

		level.spawnPositions = new List<IntVector2>();
		int remainingSpawnPositions = 40;
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				IntVector2 pos = new IntVector2(x, y);
				if (level.tiles[x, y].floor != null && level.tiles[x,y].occupant == null) {
					level.spawnPositions.Add(pos);
					remainingSpawnPositions -= 1;
				}
				if (remainingSpawnPositions <= 0)
					break;
			}
			if (remainingSpawnPositions <= 0)
				break;
		}
	}

	bool OnlyFloorAt(ref Dictionary<IntVector2, Tile> tiles, IntVector2 pos) {
		return (tiles.ContainsKey(pos) && tiles[pos].floor != null && tiles[pos].occupant == null);
	}
}
