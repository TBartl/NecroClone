using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NetMessage_SpawnOccupant : NetMessage {

    string occupant;
    IntVector2 position;
    Level level;
    int owner;

    public NetMessage_SpawnOccupant() { }
    public NetMessage_SpawnOccupant(string occupant, IntVector2 position, Level level, int owner = -2) {
        this.occupant = occupant;
        this.position = position;
        this.level = level;
        this.owner = owner;
    }

    public override bool AlsoExecuteOnServer() {
        return true;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(occupant);
        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(level.levelNum);
        writer.Write(owner);
    }

    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        occupant = reader.ReadString();
        position.x = reader.ReadInt32();
        position.y = reader.ReadInt32();
        level = LevelManager.S.GetLevel(reader.ReadInt32());
        owner = reader.ReadInt32();
        GameObject newOccupant = level.SpawnOccupant(occupant, position);
        SoundManager.S.Play(SoundManager.S.spawn);

        if (newOccupant.GetComponent<PlayerIdentity>() != null) {
            if (owner == NetManager.S.myConnectionId) {
                newOccupant.GetComponent<PlayerIdentity>().SetAsMyPlayer();
            }

            if (NetManager.S.isServer && newOccupant.name == "Player") {
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
    Level level;
    int action;
    IntVector2 direction;

    public NetMessage_ActionOccupant() { }
    public NetMessage_ActionOccupant(IntVector2 occupantPos, Level level, int action, IntVector2 direction) {
        this.occupantPos = occupantPos;
        this.level = level;
        this.action = action;
        this.direction = direction;
    }

    public override bool AlsoExecuteOnServer() {
        return true;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(occupantPos.x);
        writer.Write(occupantPos.y);
        writer.Write(level.levelNum);
        writer.Write(action);
        writer.Write(direction.x);
        writer.Write(direction.y);
    }

    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        occupantPos.x = reader.ReadInt32();
        occupantPos.y = reader.ReadInt32();
        level = LevelManager.S.GetLevel(reader.ReadInt32());
        action = reader.ReadInt32();
        direction.x = reader.ReadInt32();
        direction.y = reader.ReadInt32();
        level.tiles[occupantPos.x, occupantPos.y].occupant.GetComponent<Controller>().DoActionReal(action, direction);
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