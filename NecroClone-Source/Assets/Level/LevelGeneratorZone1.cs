using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelGen/zone1")]
public class LevelGeneratorZone1 : LevelGenerator {

	public GameObject floor;
	public GameObject wallBase;

	struct Room {
		public IntVector2 center;
		public IntVector2 size;
		public Room(IntVector2 center, IntVector2 size) {
			this.center = center;
			this.size = size;
		}
	}

	public override void GetLevel(ref Level level) {
		List<Room> rooms = new List<Room>();
		rooms.Add(new Room(new IntVector2(0, 0), new IntVector2(4, 4)));

		Dictionary<IntVector2, Tile> tiles = new Dictionary<IntVector2, Tile>();
		foreach (Room room in rooms) {
			for (int y = -room.size.y; y <= room.size.y; y++) {
				for (int x = -room.size.x; x <= room.size.x; x++) {
					IntVector2 tilePos = new IntVector2(room.center.x + x, room.center.y + y);
					Tile tile = new Tile();
					tile.floor = floor;
					if (x == Mathf.Abs(room.size.x) || y == Mathf.Abs(room.size.y)) {
						tile.occupant = wallBase;
					}
					tiles[tilePos] = tile;
				}
			}
		}

		IntVector2 bottomLeft = IntVector2.zero;
		IntVector2 topRight = IntVector2.zero;
		foreach (KeyValuePair<IntVector2, Tile> pair in tiles) {
			IntVector2 pos = pair.Key;
			bottomLeft.x = Mathf.Min(bottomLeft.x, pos.x);
			bottomLeft.y = Mathf.Min(bottomLeft.x, pos.y);
			topRight.x = Mathf.Max(topRight.x, pos.x);
			topRight.y = Mathf.Max(topRight.x, pos.y);
		}
		IntVector2 size = topRight - bottomLeft + IntVector2.one;
		level.Resize(size);
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				IntVector2 realCoord = new IntVector2(x, y) - bottomLeft;
				level.tiles[x, y] = tiles[realCoord];
			}
		}
	}
}
