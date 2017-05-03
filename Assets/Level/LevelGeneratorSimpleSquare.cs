using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "LevelGen/simpleSquare")]
public class LevelGeneratorSimpleSquare : LevelGenerator {
    public IntVector2 size;
    public float randomWallChance = .26f;

    public override Level GetLevel() {
        Level level = new Level(size);
        for (int y = 0; y < level.size.y; y++) {
            for (int x = 0; x < level.size.x; x++) {
                if (x == 0 || y == 0 || x == level.size.x - 1 || y == level.size.y - 1 || Random.value < randomWallChance)
                    level.tiles[x, y].occupant = LevelDatabase.S.GetOccupantPrefab(OccupantId.wall);
                level.tiles[x, y].floor = LevelDatabase.S.GetFloorPrefab(FloorId.basic);
            }
        }
        IntVector2 center = new IntVector2(Mathf.FloorToInt(size.x / 2f), Mathf.RoundToInt(size.y / 2f));
        level.spawnPositions.Add(center);
        level.spawnPositions.Add(center + IntVector2.up);
        level.spawnPositions.Add(center + IntVector2.right);
        level.spawnPositions.Add(center + IntVector2.up + IntVector2.right);

        return level;
    }

}
