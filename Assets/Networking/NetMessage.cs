using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public enum NetMessageID : byte {
    none, debug, clientConnectionID,
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

    public int Encode() {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(GetRecognizeByte());
        EncodeToBuffer(ref writer);

        if (stream.Length > bufferSize)
            Debug.LogError("ERROR: Message length exceeds buffer size");

        int length = (int)stream.Length;
        byte[] streamArray = stream.ToArray();
        for (int i = 0; i < length; i++) {
            buffer[i] = streamArray[i];
        }
        return length;
    }

    protected virtual void EncodeToBuffer(ref BinaryWriter writer) {

    }

    public virtual void DecodeAndExecute(ClientData clientData) {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryReader reader = new BinaryReader(stream);
        reader.ReadByte();
        DecodeBufferAndExecute(ref reader, clientData);

    }

    protected virtual void DecodeBufferAndExecute(ref BinaryReader reader, ClientData clientData) {
        DecodeBufferAndExecute(ref reader);
    }
    protected virtual void DecodeBufferAndExecute(ref BinaryReader reader) { }
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

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(message);
    }


    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        message = reader.ReadString();
        Debug.Log("Debug message recieved: " + message);
    }
}

[System.Serializable]
public class NetMessage_ClientConnectionID: NetMessage {

    public int id;

    public NetMessage_ClientConnectionID() { }
    public NetMessage_ClientConnectionID(int id) {
        this.id = id;
    }

    public override byte GetRecognizeByte() {
        return (byte)NetMessageID.clientConnectionID;
    }

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(id);
    }

    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        id = reader.ReadInt32();
        NetManager.S.myConnectionId = id;
    }
}