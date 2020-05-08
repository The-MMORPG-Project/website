using System.Xml.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocol
{
    private BinaryWriter writer;
    private BinaryReader reader;
    private MemoryStream stream;
    private byte[] buffer;

    private void InitWriter(int size)
    {
        buffer = new byte[size];
        stream = new MemoryStream(buffer);
        writer = new BinaryWriter(stream);
    }

    private void InitReader(byte[] buffer)
    {
        stream = new MemoryStream(buffer);
        reader = new BinaryReader(stream);
    }

    public byte[] Serialize(byte code, params object[] values) 
    {
        int bufferSize = 0;
        bufferSize += sizeof(byte);
        foreach (object value in values) 
        {
            Type type = value.GetType();
            if (type == typeof(float))
                bufferSize += sizeof(float);

            if (type == typeof(int))
                bufferSize += sizeof(int);
            
            if (type == typeof(string))
                bufferSize += (sizeof(char) * ((string)value).Length);
        }

        InitWriter(bufferSize);
        writer.Write(code);

        foreach (object value in values) 
        {
            Type type = value.GetType();
            if (type == typeof(float))
                writer.Write((float) value);

            if (type == typeof(int))
                writer.Write((int) value);

            if (type == typeof(string)) 
                writer.Write((string) value);
        }
        return buffer;
    }
}