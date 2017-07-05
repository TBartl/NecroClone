using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "LevelGen/simpleSquare")]
public class LevelGeneratorSimpleSquare : LevelGenerator {
    public IntVector2 size;
    public float randomWallChance = .26f;

    public override void GetLevel(ref Level level) {
        level.Resize(size);
        for (int y = 0; y < level.size.y; y++) {
            for (int x = 0; x < level.size.x; x++) {
                if (x == 0 || y == 0 || x == level.size.x - 1 || y == level.size.y - 1 || Random.value < randomWallChance)
                    level.tiles[x, y].occupant = LevelDatabase.S.GetOccupantPrefab("WallBasic");
                level.tiles[x, y].floor = LevelDatabase.S.GetFloorPrefab("Basic");
            }
        }
        IntVector2 center = new IntVector2(Mathf.FloorToInt(size.x / 2f), Mathf.RoundToInt(size.y / 2f));

        for (int y = -2; y <= 2; y++) {
            for (int x = -2; x <= 2; x++) {
                level.spawnPositions.Add(center + new IntVector2(x,y));
            }
        }

        level.tiles[center.x, center.y].occupant = LevelDatabase.S.GetOccupantPrefab("Spawner");
        
    }

}
