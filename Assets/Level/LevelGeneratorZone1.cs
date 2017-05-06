using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OccupantAndSpawnChance {
    public OccupantId occupant;
    public float chance;
}

[CreateAssetMenu(menuName = "LevelGen/zone1")]
public class LevelGeneratorZone1 : LevelGenerator {
    public int roomSize = 7;
    public int roomsInRow = 3;

    public List<OccupantAndSpawnChance> occupantSpawns; 


    public override void GetLevel(ref Level level) {
        int size = roomsInRow * roomSize + roomsInRow + 1; 
        level.Resize(new IntVector2(size, size));
        for (int y = 0; y < level.size.y; y++) {
            for (int x = 0; x < level.size.x; x++) {
                level.tiles[x, y].occupant = LevelDatabase.S.GetOccupantPrefab(OccupantId.wall);
                level.tiles[x, y].floor = LevelDatabase.S.GetFloorPrefab(FloorId.basic);
            }
        }

        for (int yRoom = 0; yRoom < roomsInRow; yRoom++) {
            for (int xRoom = 0; xRoom < roomsInRow; xRoom++) {
                for (int y = 0; y < roomSize; y++) {
                    for (int x = 0; x < roomSize; x++) {
                        IntVector2 realpos = 
                            new IntVector2(xRoom * (roomSize + 1), yRoom * (roomSize + 1)) + 
                            new IntVector2(1,1) +
                            new IntVector2(x, y);
                        level.tiles[realpos.x, realpos.y].occupant = null;
                        if (xRoom == 0 && yRoom == 0)
                            continue;

                        foreach(OccupantAndSpawnChance occupantSpawn in occupantSpawns) {
                            if (Random.value <= occupantSpawn.chance) {
                                level.tiles[realpos.x, realpos.y].occupant = LevelDatabase.S.GetOccupantPrefab(occupantSpawn.occupant);
                                break;
                            }
                        }

                    }
                }
            }
        }

        int spawnCenterInt = 1 + Mathf.FloorToInt(roomSize / 2f);
        IntVector2 spawnCenter = new IntVector2(spawnCenterInt, spawnCenterInt);
        for (int y = -2; y <= 2; y++) {
            for (int x = -2; x <= 2; x++) {
                level.spawnPositions.Add(spawnCenter + new IntVector2(x, y));
            }
        }

        //level.tiles[center.x, center.y].occupant = LevelDatabase.S.GetOccupantPrefab(OccupantId.spawner);

    }

}
