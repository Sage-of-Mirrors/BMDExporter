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
            // Since TWW is backwards when it comes to winding order, we're going to have to swap our vertexes.
            // We start with a list of list<shorts> to hold our vertexes so we can ignore how the vertex is laid out.
            List<List<short>> hah = new List<List<short>>();

            // Process each vertex here. Each vert is a sequence of "pos/uv/normal" or something similar.
            for (int i = 1; i < 4; i++)
            {
                List<short> blah = new List<short>();

                // get pos, uv, normal, etc data where applicable
                string[] indexes = source[i].Split('/');

                // get each index and add to the local index list
                for (int j = 0; j < indexes.Length; j++)
                {
                    blah.Add((short)(Convert.ToInt16(indexes[j]) - 1));
                }

                // We're finished with this vertex, so add it to the mast list
                hah.Add(blah);
            }

            // To flip the winding order, we swizzle vertex 0 and 2.
            m_triIndexes.AddRange(hah[2].ToArray());
            m_triIndexes.AddRange(hah[1].ToArray());
            m_triIndexes.AddRange(hah[0].ToArray());

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
