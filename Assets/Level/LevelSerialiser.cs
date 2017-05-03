using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelSerializer {
    public int numMessages;
    public byte[] serialised;

    public void Serialise() {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        for (int y = 0; y < LevelManager.S.level.size.y; y++) {
            for (int x = 0; x < LevelManager.S.level.size.x; x++) {
                GameObject floor = LevelManager.S.level.tiles[x, y].floor;
                if (floor == null)
                    writer.Write((byte)0);
                else
                    writer.Write((byte)floor.GetComponent<DatabaseID>().GetID());

                GameObject occupant = LevelManager.S.level.tiles[x, y].occupant;
                if (occupant == null)
                    writer.Write((byte)0);
                else
                    writer.Write((byte)occupant.GetComponent<DatabaseID>().GetID());
            }
        }

        serialised = stream.ToArray();
    }

    public void DeSerialise() {
        MemoryStream stream = new MemoryStream(serialised);
        BinaryReader reader = new BinaryReader(stream);
        for (int y = 0; y < LevelManager.S.level.size.y; y++) {
            for (int x = 0; x < LevelManager.S.level.size.x; x++) {
                LevelManager.S.level.tiles[x, y].floor = LevelDatabase.S.GetFloorPrefab((FloorId)reader.ReadByte());
                LevelManager.S.level.tiles[x, y].occupant = LevelDatabase.S.GetOccupantPrefab((OccupantId)reader.ReadByte());
            }
        }

        serialised = stream.ToArray();
    }

    public int GetRequiredNumOfPieces() {
        int top = serialised.Length;
        int bot = (NetMessage.bufferSize - 1);
        return (top - 1) / bot + 1; // Integer division with ceil is weird
    }
}
