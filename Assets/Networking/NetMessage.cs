using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public enum NetMessageID : byte {
    none, debug,
    startSendLevel, sendLevelPiece,
    clientInput,
    spawnOccupant, actionOccupant
}

[System.Serializable]
public class NetMessage {
    public static int bufferSize = 1024;
    public static byte[] buffer = new byte[bufferSize];

    public virtual byte GetRecognizeByte() {
        return (byte)NetMessageID.none;
    }
    public virtual bool AlsoExecuteOnServer() {
        return false;
    }

    public virtual bool IsThisMessage() {
        return buffer[0] == GetRecognizeByte();
    }

    public virtual void EncodeToBuffer() {

    }   

    protected virtual void DecodeBufferAndExecute() { }

    public virtual void DecodeBufferAndExecute(ClientData clientData) {
        DecodeBufferAndExecute();
    }
}

[System.Serializable]
public class NetMessageDebug : NetMessage {

    public string message;

    public NetMessageDebug() { }
    public NetMessageDebug(string message) {
        this.message = message;
    }

    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.debug;
    }

    public override void EncodeToBuffer() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
        writer.Write(message);
    }

    protected override void DecodeBufferAndExecute() {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        message = reader.ReadString();
        Debug.Log("Debug message recieved: " + message);
    }
}