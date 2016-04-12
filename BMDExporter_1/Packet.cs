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
        public int m_triCount;
        public int shortCount;
        public int numVertexes;
        public List<short> m_triIndexes;

        public Packet()
        {
            m_triIndexes = new List<short>();
        }

        public void AddTriangle(string[] source)
        {
            // Since TWW is backwards when it comes to winding order, we're going to have to swap our vertexes.
            // We start with a list of list<shorts> to hold our vertexes so we can ignore how the vertex is laid out.
            List<List<short>> vertices = new List<List<short>>();

            // Process each vertex here. Each vert is a sequence of "pos/uv/normal" or something similar.
            for (int i = 1; i < 4; i++)
            {
                List<short> localVert = new List<short>();

                // get pos, uv, normal, etc data where applicable
                string[] indexes = source[i].Split('/');

                // get each index and add to the local index list
                for (int j = 0; j < indexes.Length; j++)
                {
                    localVert.Add((short)(Convert.ToInt16(indexes[j]) - 1));
                }

                // We're finished with this vertex, so add it to the mast list
                vertices.Add(localVert);
            }

            // To flip the winding order, we swizzle vertex 0 and 2.
            m_triIndexes.AddRange(vertices[2].ToArray());
            m_triIndexes.AddRange(vertices[1].ToArray());
            m_triIndexes.AddRange(vertices[0].ToArray());

            m_triCount++;
        }

        public void WritePacket(EndianBinaryWriter writer)
        {
            writer.Write((byte)0x90);
            writer.Write((short)numVertexes);

            foreach (short sho in m_triIndexes)
                writer.Write(sho);
        }

        public void WriteSize(EndianBinaryWriter writer)
        {
            int size = (numVertexes * shortCount * 2) + 3;
            writer.Write(size);
            writer.Write((int)0);
        }


        public short GetSize()
        {
            return (short)((numVertexes * shortCount * 2) + 3);
        }
    }
}
