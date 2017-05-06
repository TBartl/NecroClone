using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelSerializer {
    public Level toSerializeTo;
    public int numMessages;
    public byte[] serialised;

    public void Serialise(Level level) {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        for (int y = 0; y < level.size.y; y++) {
            for (int x = 0; x < level.size.x; x++) {
                GameObject floor = level.tiles[x, y].floor;
                if (floor == null)
                    writer.Write((byte)0);
                else
                    writer.Write((byte)floor.GetComponent<DatabaseID>().GetID());

                GameObject occupant = level.tiles[x, y].occupant;
                if (occupant == null)
                    writer.Write((byte)0);
                else {
                    writer.Write((byte)occupant.GetComponent<DatabaseID>().GetID());
                    ChangeableProperty[] occupantProperties = occupant.GetComponents<ChangeableProperty>();
                    //writer.Write(occupantProperties.Length); Don't need to write length
                    foreach(ChangeableProperty property in occupantProperties) {
                        property.Encode(ref writer);
                    }
                }
            }
        }

        serialised = stream.ToArray();
    }

    public void DeSerialise() {
        MemoryStream stream = new MemoryStream(serialised);
        BinaryReader reader = new BinaryReader(stream);
        for (int y = 0; y < toSerializeTo.size.y; y++) {
            for (int x = 0; x < toSerializeTo.size.x; x++) {
                toSerializeTo.tiles[x, y].floor = LevelDatabase.S.GetFloorPrefab((FloorId)reader.ReadByte());

                OccupantId occupantID = (OccupantId)reader.ReadByte();
                toSerializeTo.tiles[x, y].occupant = LevelDatabase.S.GetOccupantPrefab(occupantID);
                toSerializeTo.tiles[x, y].Draw(new IntVector2(x, y), toSerializeTo.transform);

                if (occupantID != OccupantId.none) {
                    ChangeableProperty[] occupantProperties = toSerializeTo.tiles[x, y].occupant.GetComponents<ChangeableProperty>();
                    foreach (ChangeableProperty property in occupantProperties) {
                        property.Decode(ref reader);
                    }
                }
            }
        }

        //toSerializeTo.Draw(); Don't need to draw since we are drawing as we go
    }

    public int GetRequiredNumOfPieces() {
        int top = serialised.Length;
        int bot = (NetMessage.bufferSize - 1);
        return (top - 1) / bot + 1; // Integer division with ceil is weird
    }
}
