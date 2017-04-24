using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "LevelGen/simpleSquare")]
public class LevelGeneratorSimpleSquare : LevelGenerator {
    public IntVector2 size;

    public override Level GetLevel() {
        Level level = new Level(size);
        for (int y = 0; y < level.size.y; y++) {
            for (int x = 0; x < level.size.x; x++) {
                if (x == 0 || y == 0 || x == level.size.x - 1 || y == level.size.y - 1)
                    level.tiles[x, y].occupant = LevelDatabase.S.walls.basic;
                level.tiles[x, y].floor = LevelDatabase.S.floor.basic;
            }
        }
        level.tiles[Mathf.RoundToInt(size.x / 2f), Mathf.RoundToInt(size.y / 2f)].occupant = LevelDatabase.S.entities.player;
        return level;
    }

}
