using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class NetMessage_StartSendLevel : NetMessage {
    
    public NetMessage_StartSendLevel() { }

    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.startSendLevel;
    }

    public override void EncodeToBuffer() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
        writer.Write((int)LevelManager.S.level.size.x);
        writer.Write((int)LevelManager.S.level.size.y);

        writer.Write(LevelManager.S.serializer.serialised.Length);
    }

    protected override void DecodeBufferAndExecute() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        LevelManager.S.level.size.x = reader.ReadInt32();
        LevelManager.S.level.size.y = reader.ReadInt32();
        LevelManager.S.level.tiles = new Tile[LevelManager.S.level.size.x, LevelManager.S.level.size.y];

        int serialisedLength = reader.ReadInt32();
        LevelManager.S.serializer.serialised = new byte[serialisedLength];
        LevelManager.S.serializer.numMessages = 0;
    }
}

[System.Serializable]
public class NetMessage_SendLevelPiece : NetMessage {

    int pieceNumber;
    public NetMessage_SendLevelPiece() { }
    public NetMessage_SendLevelPiece(int pieceNumber) {
        this.pieceNumber = pieceNumber;
    }

    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.sendLevelPiece;
    }

    public override void EncodeToBuffer() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
        for (int i = 0; i < NetMessage.bufferSize - 1; i++) {
            int realIndex = i + pieceNumber * (NetMessage.bufferSize - 1);
            if (realIndex >= LevelManager.S.serializer.serialised.Length)
                break;
            writer.Write(LevelManager.S.serializer.serialised[realIndex]);
        }
    }

    protected override void DecodeBufferAndExecute() {
        pieceNumber = LevelManager.S.serializer.numMessages;
        LevelManager.S.serializer.numMessages += 1;

        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        for (int i = 0; i < NetMessage.bufferSize - 1; i++) {
            int realIndex = i + pieceNumber * (NetMessage.bufferSize - 1);
            if (realIndex >= LevelManager.S.serializer.serialised.Length)
                break;
            LevelManager.S.serializer.serialised[realIndex] = reader.ReadByte();
        }

        if (LevelManager.S.serializer.numMessages >= LevelManager.S.serializer.GetRequiredNumOfPieces()) {
            LevelManager.S.serializer.DeSerialise();
            LevelManager.S.level.Draw();
        }
    }
}