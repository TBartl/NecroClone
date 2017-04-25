using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
    public IntVector2 size;
    public Tile[,] tiles;

    public Level(IntVector2 size) {
        this.size = size;
        tiles = new Tile[size.x, size.y];
    }

    public void Draw() {
        GameObject parent = new GameObject("Level");
        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                tiles[x, y].Draw(new IntVector2(x, y), parent.transform);
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

}
