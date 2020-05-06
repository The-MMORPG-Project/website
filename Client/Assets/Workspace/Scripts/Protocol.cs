using System.Xml.Linq;
using System;
using System.IO;

namespace Server
{
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

        public byte[] Serialize(byte code, string value) 
        {
            int bufferSize = sizeof(byte) + (sizeof(Char) * value.Length);
            InitWriter(bufferSize);
            writer.Write(code);
            writer.Write(value);
            return buffer;
        }

        public void Deserialize(byte[] buffer, out byte code, out string value) 
        {
            InitReader(buffer);
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;
            code = reader.ReadByte();
            value = reader.ReadString();
        }
    }
}