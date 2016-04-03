using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class Packet
    {
        int m_triCount;
        List<short> m_triIndexes;

        public Packet()
        {
            m_triIndexes = new List<short>();
        }

        public void AddTriangle(string[] source)
        {
            for (int i = 1; i < 4; i++)
            {
                string[] indexes = source[i].Split('/');

                for (int j = 0; j < indexes.Length; j++)
                {
                    m_triIndexes.Add((short)(Convert.ToInt16(indexes[j]) - 1));
                }
            }

            m_triCount++;
        }

        public void WritePacket(EndianBinaryWriter writer)
        {
            writer.Write((byte)0x90);
            writer.Write((short)(m_triIndexes.Count / 2));

            foreach (short sho in m_triIndexes)
                writer.Write(sho);
        }

        public void WriteSize(EndianBinaryWriter writer)
        {
            int size = m_triIndexes.Count * 2 + 3;
            writer.Write(size);
            writer.Write((int)0);
        }


        public short GetSize()
        {
            return (short)(m_triIndexes.Count * 2 + 3);
        }
    }
}
