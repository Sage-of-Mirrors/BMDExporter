using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class Fog
    {
        public byte Type;
        public bool Enable;
        public ushort Center;
        public float StartZ;
        public float EndZ;
        public float NearZ;
        public float FarZ;
        public Color Color;
        public ushort[] Table;

        public Fog()
        {
            Table = new ushort[10];
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(Enable);
            writer.Write(Center);
            writer.Write(StartZ);
            writer.Write(EndZ);
            writer.Write(NearZ);
            writer.Write(FarZ);
            writer.Write((byte)(Color.R * 255.0f));
            writer.Write((byte)(Color.G * 255.0f));
            writer.Write((byte)(Color.B * 255.0f));
            writer.Write((byte)(Color.A * 255.0f));
            for (int i = 0; i < 10; i++)
                writer.Write(Table[i]);
        }
    }
}
