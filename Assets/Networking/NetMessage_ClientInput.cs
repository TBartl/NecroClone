﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class NetMessage_ClientInput : NetMessage {

    public PlayerInputKey inputKey;

    public NetMessage_ClientInput() { }
    public NetMessage_ClientInput(PlayerInputKey inputKey) {
        this.inputKey = inputKey;
    }
    
    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.clientInput;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write((byte)inputKey);
    }

    public override void DecodeBufferAndExecute(ClientData clientData) {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        inputKey = (PlayerInputKey)reader.ReadByte();
        
        if (inputKey == PlayerInputKey.space && clientData.player == null) {
            IntVector2 spawnPos = LevelManager.S.startLevel.GetOpenPlayerSpawnPosition();
            if (spawnPos == IntVector2.error) {
                Debug.LogError("No available spawn position!");
                return;
            }
            NetManager.S.SendServerMessageToAll(new NetMessage_SpawnOccupant(OccupantId.player, spawnPos, LevelManager.S.startLevel, clientData.connectionID));
        }
        else if (clientData.player != null) {
            clientData.player.GetComponent<PlayerController>().OnInput(inputKey);
        }
    }
}