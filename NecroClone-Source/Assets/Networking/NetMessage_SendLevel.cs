﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class NetMessage_StartSendLevel : NetMessage {
    Level level;
    public NetMessage_StartSendLevel() { }
    public NetMessage_StartSendLevel(Level level) {
        this.level = level;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(level.levelNum);
        writer.Write(level.size.x);
        writer.Write(level.size.y);

        writer.Write(LevelManager.S.serializer.serialised.Length);
    }

    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        int levelIndex = reader.ReadInt32();
        while (LevelManager.S.levels.Count < levelIndex + 1)
            LevelManager.S.CreateNewLevel();
        level = LevelManager.S.GetLevel(levelIndex);
        level.size.x = reader.ReadInt32();
        level.size.y = reader.ReadInt32();
        level.tiles = new Tile[level.size.x, level.size.y];
        int serialisedLength = reader.ReadInt32();
        LevelManager.S.serializer.serialised = new byte[serialisedLength];
        LevelManager.S.serializer.numMessages = 0;
        LevelManager.S.serializer.toSerializeTo = level;
    }
}

[System.Serializable]
public class NetMessage_SendLevelPiece : NetMessage {

    int pieceNumber;
    public NetMessage_SendLevelPiece() { }
    public NetMessage_SendLevelPiece(int pieceNumber) {
        this.pieceNumber = pieceNumber;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        for (int i = 0; i < NetMessage.bufferSize - 1; i++) {
            int realIndex = i + pieceNumber * (NetMessage.bufferSize - 1);
            if (realIndex >= LevelManager.S.serializer.serialised.Length)
                break;
            writer.Write(LevelManager.S.serializer.serialised[realIndex]);
        }
    }

    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {

        pieceNumber = LevelManager.S.serializer.numMessages;
        LevelManager.S.serializer.numMessages += 1;

        for (int i = 0; i < NetMessage.bufferSize - 1; i++) {
            int realIndex = i + pieceNumber * (NetMessage.bufferSize - 1);
            if (realIndex >= LevelManager.S.serializer.serialised.Length)
                break;
            LevelManager.S.serializer.serialised[realIndex] = reader.ReadByte();
        }

        if (LevelManager.S.serializer.numMessages >= LevelManager.S.serializer.GetRequiredNumOfPieces()) {
            LevelManager.S.serializer.DeSerialise();
        }
    }
}