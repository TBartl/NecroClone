using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public static class NetMessageMaintainer
{
    static List<System.Type> netMessageTypes;

    public static void Setup()
    {
        netMessageTypes = new List<System.Type>();
        System.Type baseType = typeof(NetMessage);
        System.Reflection.Assembly assembly = baseType.Assembly;
        System.Type[] types = assembly.GetTypes();
        foreach (System.Type type in types)
        {
            if (baseType.IsAssignableFrom(type))
            {
                netMessageTypes.Add(type);
            }
        }
    }

    public static byte GetRecognizeByte(System.Type type)
    {
        if (netMessageTypes == null)
            Setup();
        return (byte)netMessageTypes.IndexOf(type);
    }

    public static NetMessage GetFromRecognizeByte(byte b)
    {
        if (netMessageTypes == null)
            Setup();
        return (NetMessage)System.Activator.CreateInstance(netMessageTypes[b]);
    }
}

[System.Serializable]
public class NetMessage {
    public static int bufferSize = 1024;
    public static byte[] buffer = new byte[bufferSize];

    public byte GetRecognizeByte() {
        return NetMessageMaintainer.GetRecognizeByte(this.GetType());
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

    protected override void EncodeToBuffer(ref BinaryWriter writer) {
        writer.Write(message);
    }


    protected override void DecodeBufferAndExecute(ref BinaryReader reader) {
        message = reader.ReadString();
        Debug.Log("Debug message recieved: " + message);
    }
}