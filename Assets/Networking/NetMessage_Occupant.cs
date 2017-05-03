using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NetMessage_SpawnOccupant : NetMessage {

    OccupantId occupant;
    IntVector2 position;

    public NetMessage_SpawnOccupant() { }
    public NetMessage_SpawnOccupant(OccupantId occupant, IntVector2 position) {
        this.occupant = occupant;
        this.position = position;
    }
    
    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.spawnOccupant;
    }

    public override bool AlsoExecuteOnServer() {
        return true;
    }

    public override void EncodeToBuffer() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
        writer.Write((byte)occupant);
        writer.Write(position.x);
        writer.Write(position.y);
    }

    protected override void DecodeBufferAndExecute() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        occupant = (OccupantId)reader.ReadByte();
        position.x = reader.ReadInt32();
        position.y = reader.ReadInt32();
        LevelManager.S.level.AddOccupant(occupant, position);
    }
}

[System.Serializable]
public class NetMessage_ActionOccupant : NetMessage {

    OccupantId occupant;

    public NetMessage_ActionOccupant() { }
    //public NetMessage_ActionOccupant(OccupantId occupant) {
    //    this.occupant = occupant;
    //}


    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.actionOccupant;
    }

    public override bool AlsoExecuteOnServer() {
        return true;
    }

    public override void EncodeToBuffer() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
    }

    protected override void DecodeBufferAndExecute() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
    }
}


//[Command]
//public void CmdSpawnPlayer() {
//    if (playerInstance != null)
//        return;
//    foreach (IntVector2 pos in LevelManager.S.level.spawnPositions) {
//        if (LevelManager.S.level.Occuppied(pos) == false) {
//            playerInstance = (GameObject)GameObject.Instantiate(playerPrefab, LevelManager.S.level.parent);
//            Movable mov = playerInstance.GetComponent<Movable>();
//            mov.SetPos(pos);
//            NetworkServer.Spawn(playerInstance);
//            break;
//        }
//    }
//}