using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NetMessage_SpawnOccupant : NetMessage {

    OccupantId occupant;
    IntVector2 position;
    int owner;

    public NetMessage_SpawnOccupant() { }
    public NetMessage_SpawnOccupant(OccupantId occupant, IntVector2 position, int owner = -2) {
        this.occupant = occupant;
        this.position = position;
        this.owner = owner;
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
        writer.Write(owner);
    }

    protected override void DecodeBufferAndExecute() {
        Debug.Log("Occupant");
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        occupant = (OccupantId)reader.ReadByte();
        position.x = reader.ReadInt32();
        position.y = reader.ReadInt32();
        owner = reader.ReadInt32();
        GameObject newOccupant = LevelManager.S.level.AddOccupant(occupant, position);
        SoundManager.S.Play(SoundManager.S.spawn);

        if (newOccupant.GetComponent<PlayerIdentity>() != null) {
            if (owner == NetManager.S.myConnectionId) {
                newOccupant.GetComponent<PlayerIdentity>().SetAsMyPlayer();
            }

            if (NetManager.S.isServer && newOccupant.GetComponent<DatabaseID_Occupant>().GetID() == (byte)OccupantId.player) {
                if (NetManager.S.GetThisServerClient().connectionID == owner)
                    NetManager.S.GetThisServerClient().player = newOccupant;
                foreach (ClientData client in NetManager.S.GetClients()) {
                    if (client.connectionID == owner) {
                        client.player = newOccupant;
                        break;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class NetMessage_ActionOccupant : NetMessage {

    IntVector2 occupantPos;
    int action;
    IntVector2 direction;

    public NetMessage_ActionOccupant() { }
    public NetMessage_ActionOccupant(IntVector2 occupantPos, int action, IntVector2 direction) {
        this.occupantPos = occupantPos;
        this.action = action;
        this.direction = direction;
    }
    
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
        writer.Write(occupantPos.x);
        writer.Write(occupantPos.y);
        writer.Write(action);
        writer.Write(direction.x);
        writer.Write(direction.y);
    }

    protected override void DecodeBufferAndExecute() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        occupantPos.x = reader.ReadInt32();
        occupantPos.y = reader.ReadInt32();
        action = reader.ReadInt32();
        direction.x = reader.ReadInt32();
        direction.y = reader.ReadInt32();
        LevelManager.S.level.tiles[occupantPos.x, occupantPos.y].occupant.GetComponent<Controller>().DoActionReal(action, direction);
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