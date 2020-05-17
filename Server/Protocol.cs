using System;
using System.IO;

public class Protocol : IDisposable
{
    private BinaryWriter writer;
    private MemoryStream stream;
    private byte[] buffer;

    private void InitWriter(int size)
    {
        buffer = new byte[size];
        stream = new MemoryStream(buffer);
        writer = new BinaryWriter(stream);
    }

    public byte[] Serialize(byte code, params object[] values)
    {
        int bufferSize = 0;
        bufferSize += sizeof(byte);
        foreach (object value in values)
        {
            Type type = value.GetType();

            if (type == typeof(int) || type == typeof(uint))
                bufferSize += sizeof(int);

            if (type == typeof(float))
                bufferSize += sizeof(float);

            if (type == typeof(string))
                bufferSize += (sizeof(char) * ((string)value).Length);

<<<<<<< HEAD
            if (type == typeof(byte) || type.IsEnum)
=======
            if (type == typeof(Valk.Networking.ErrorType))
>>>>>>> aebe5890a4ae38df6600cca8d6b441198077db01
                bufferSize += sizeof(byte);
        }

        InitWriter(bufferSize);
        writer.Write(code);

        foreach (object value in values)
        {
            Type type = value.GetType();
            if (type == typeof(uint))
                writer.Write((uint)value);

            if (type == typeof(float))
                writer.Write((float) value);

            if (type == typeof(int))
                writer.Write((int)value);

            if (type == typeof(string))
                writer.Write((string)value);

<<<<<<< HEAD
            if (type == typeof(byte))
                writer.Write((byte)value);

            if (type.IsEnum)
                 writer.Write((byte)(int)value);
=======
            if (type == typeof(Valk.Networking.ErrorType))
                writer.Write((byte)value); // Casted as byte for consistency with Packet.cs
>>>>>>> aebe5890a4ae38df6600cca8d6b441198077db01
        }
        return buffer;
    }

    // Flag: Has Dispose already been called?
    bool disposed = false;

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            writer.Dispose();
            stream.Dispose();
        }

        disposed = true;
    }
}