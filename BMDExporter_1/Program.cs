using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    public enum HierarchyDataTypes : ushort
    {
        Finish = 0x0, //Terminator
        NewNode = 0x01, //Hierarchy down (insert node), new child
        EndNode = 0x02, //Hierarchy up, close child
        Joint = 0x10, //
        Material = 0x11,
        Shape = 0x12, //ie Batch
    }

    class Program
    {
        static List<Batch> m_batches = new List<Batch>();
        static List<Vector3> m_vertexes = new List<Vector3>();
        static List<Vector2> m_texCoords = new List<Vector2>();
        static List<Vector3> m_normals = new List<Vector3>();
        static List<AttributeType> m_totalAttribs = new List<AttributeType>() { AttributeType.Position };
        static List<BinaryTextureImage> m_textures = new List<BinaryTextureImage>();

        static StreamReader m_materialReader = null;

        const string padString = "This is padding data to align.";
        static string FileName;

        static Mat3Container MatContainer = new Mat3Container();

        static void Main(string[] args)
        {
            //args[0] = @"C:\Program Files (x86)\SZS Tools\TestCube2\untitled.obj";
            //if (args.Length == 0)
                //return;

            //FileName = args[0];
            FileName = @"C:\Users\Dylan\Documents\BMDExporter\BMDExporter_1\bin\Debug\links_slide_of_fun.obj";

            if (FileName.EndsWith(".obj"))
            {
                using (StreamReader reader = new StreamReader(FileName))
                {
                    LoadOBJ(reader);
                }
            }
            /*
            using (StreamReader reader = new StreamReader(args[0]))
            {
                Batch curBatch = new Batch();
                curBatch.SetName("test");
                Packet curPack = new Packet();
                BoundingBox box = null;
                List<Vector3> localVerts = new List<Vector3>();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] decompLine = line.Split(' ');

                    switch(decompLine[0])
                    {
                        case "v":
                            localVerts.Add(ParseVec3(decompLine));
                            break;
                        case "vt":
                            if (!curBatch.HasAttribute(AttributeType.Tex0))
                                curBatch.AddAttribute(AttributeType.Tex0);
                            if (!m_totalAttribs.Contains(AttributeType.Tex0))
                                m_totalAttribs.Add(AttributeType.Tex0);
                            m_texCoords.Add(ParseVec2(decompLine));
                            break;
                        case "vn":
                            if (!curBatch.HasAttribute(AttributeType.Normal))
                                curBatch.AddAttribute(AttributeType.Normal);
                            if (!m_totalAttribs.Contains(AttributeType.Normal))
                                m_totalAttribs.Add(AttributeType.Normal);
                            m_normals.Add(ParseVec3(decompLine));
                            break;
                        case "f":
                            curPack.AddTriangle(decompLine);
                            break;
                        case "o":
                        case "g":
                            if (curBatch != null)
                            {
                                box = new BoundingBox(localVerts);
                                curBatch.SetBoundingBox(box);
                                curBatch.CreateBone();
                                curBatch.AddPacket(curPack);
                                m_vertexes.AddRange(localVerts);
                                localVerts.Clear();

                                m_batches.Add(curBatch);
                            }
                            curBatch = new Batch();
                            curPack = new Packet();
                            curBatch.SetName(decompLine[1]);
                            break;
                        case "mtllib":
                            m_materialReader = new StreamReader(Path.GetDirectoryName(args[0]) + @"\" + decompLine[1]);
                            break;
                        case "usemtl":
                            if (m_materialReader == null)
                            {
                                Console.Write("Material reader was null!");
                                continue;
                            }

                            curBatch.SetMaterial(new Material(decompLine[1], m_materialReader, Path.GetDirectoryName(args[0])));
                            MatContainer.Materials.Add(curBatch.Material);
                            for (int i = 0; i < 8; i++)
                            {
                                if (curBatch.Material.Textures[i] != null)
                                    m_textures.Add(curBatch.Material.Textures[i]);
                            }
                            break;
                    }
                }

                box = new BoundingBox(localVerts);
                curBatch.SetBoundingBox(box);
                curBatch.CreateBone();
                curBatch.AddPacket(curPack);
                m_vertexes.AddRange(localVerts);
                localVerts.Clear();
                m_totalAttribs.Add(AttributeType.NullAttr);

                m_batches.Add(curBatch);
            }*/

            MemoryStream inf1 = new MemoryStream();
            EndianBinaryWriter inf1Writer = new EndianBinaryWriter(inf1, Endian.Big);
            WriteInf1(inf1Writer);

            MemoryStream vtx1 = new MemoryStream();
            EndianBinaryWriter vtx1Writer = new EndianBinaryWriter(vtx1, Endian.Big);
            WriteVtx1(vtx1Writer);

            MemoryStream evp1 = new MemoryStream();
            EndianBinaryWriter evp1Writer = new EndianBinaryWriter(evp1, Endian.Big);
            WriteEvp1(evp1Writer);

            MemoryStream drw1 = new MemoryStream();
            EndianBinaryWriter drw1Writer = new EndianBinaryWriter(drw1, Endian.Big);
            WriteDrw1(drw1Writer);

            MemoryStream jnt1 = new MemoryStream();
            EndianBinaryWriter jnt1Writer = new EndianBinaryWriter(jnt1, Endian.Big);
            WriteJnt1(jnt1Writer);

            MemoryStream shp1 = new MemoryStream();
            EndianBinaryWriter shp1Writer = new EndianBinaryWriter(shp1, Endian.Big);
            WriteShp1(shp1Writer);

            MemoryStream mat3 = new MemoryStream();
            EndianBinaryWriter mat3Writer = new EndianBinaryWriter(mat3, Endian.Big);
            WriteMat3(mat3Writer);

            MemoryStream tex1 = new MemoryStream();
            EndianBinaryWriter tex1Writer = new EndianBinaryWriter(tex1, Endian.Big);
            WriteTex1(tex1Writer);

            string outputPath = Path.GetDirectoryName(FileName) + @"\" + "model.bmd";

            using (FileStream testJnt = new FileStream(outputPath, FileMode.Create))
            {
                EndianBinaryWriter testWriter = new EndianBinaryWriter(testJnt, Endian.Big);

                WriteHeader(testWriter);
                testWriter.Write(inf1.ToArray());
                testWriter.Write(vtx1.ToArray());
                testWriter.Write(evp1.ToArray());
                testWriter.Write(drw1.ToArray());
                testWriter.Write(jnt1.ToArray());
                testWriter.Write(shp1.ToArray());
                testWriter.Write(mat3.ToArray());
                testWriter.Write(tex1.ToArray());

                testWriter.BaseStream.Seek(0x8, 0);
                testWriter.Write((int)testWriter.BaseStream.Length);
            }
        }
 
        private static void LoadOBJ(StreamReader reader)
        {
            Batch curBatch = null;
            //curBatch.SetName("test");
            List<List<short>> localFaceIndexesList = new List<List<short>>();
            List<AttributeType> localAttribs = new List<AttributeType>();

            while(!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parsedLine = line.Split(' ');

                switch (parsedLine[0])
                {
                    // Vertex attributes
                    case "v":
                        m_vertexes.Add(ParseVec3(parsedLine));
                        break;
                    case "vt":
                        m_texCoords.Add(ParseVec2(parsedLine));
                        if (!m_totalAttribs.Contains(AttributeType.Tex0))
                            m_totalAttribs.Add(AttributeType.Tex0);
                        if (!localAttribs.Contains(AttributeType.Tex0))
                            localAttribs.Add(AttributeType.Tex0);
                        break;
                    case "vn":
                        m_normals.Add(ParseVec3(parsedLine));
                        if (!localAttribs.Contains(AttributeType.Normal))
                            localAttribs.Add(AttributeType.Normal);
                        break;
                    case "f":
                        List<List<short>> test = new List<List<short>>();
                        for (int blu = 1; blu < 4; blu++)
                        {
                            List<short> locIndexes = new List<short>();
                            string[] indexes = parsedLine[blu].Split('/');
                            for (int i = 0; i < indexes.Length; i++)
                            {
                                locIndexes.Add((short)(Convert.ToInt16(indexes[i]) - 1));
                            }
                            curBatch.localVerts.Add(m_vertexes[locIndexes[0]]);
                            test.Add(locIndexes);
                        }
                        List<short> temp = new List<short>(test[0]);
                        test[0] = test[2];
                        test[2] = temp;
                        localFaceIndexesList.AddRange(test.ToArray());
                        break;
                    case "usemtl":
                        #region Start new batch
                        if (curBatch != null)
                        {
                            Packet pac = new Packet();
                            for (int i = 0; i < localFaceIndexesList.Count; i++)
                            {
                                pac.m_triIndexes.AddRange(localFaceIndexesList[i].ToArray());
                            }
                            pac.m_triCount = localFaceIndexesList.Count / localFaceIndexesList[0].Count;
                            localFaceIndexesList.Clear();
                            curBatch.MakeBoundingBox();
                            curBatch.AddPacket(pac);
                            curBatch.m_activeAttribs.AddRange(localAttribs);
                            localAttribs.Clear();
                            localAttribs.Add(AttributeType.Position);
                        }
                        else
                        {
                            curBatch = new Batch();
                            curBatch.SetName(parsedLine[1]);
                            curBatch.CreateBone();
                        }
                        #endregion
                        #region Get material
                        if (m_materialReader == null)
                        {
                            Console.Write("Material reader was null!");
                            continue;
                        }

                        curBatch.SetMaterial(new Material(parsedLine[1], m_materialReader, Path.GetDirectoryName(FileName)));
                        MatContainer.Materials.Add(curBatch.Material);
                        for (int i = 0; i < 8; i++)
                        {
                            if (curBatch.Material.Textures[i] != null)
                                m_textures.Add(curBatch.Material.Textures[i]);
                        }
                        #endregion
                        if (curBatch.localVerts.Count != 0)
                        {
                            m_batches.Add(curBatch);
                            curBatch = new Batch();
                            curBatch.SetName(parsedLine[1]);
                        }
                        break;
                    case "mtllib":
                        m_materialReader = new StreamReader(Path.GetDirectoryName(FileName) + @"\" + parsedLine[1]);
                        break;
                }
            }

            Packet finalPac = new Packet();
            for (int i = 0; i < localFaceIndexesList.Count; i++)
            {
                finalPac.m_triIndexes.AddRange(localFaceIndexesList[i].ToArray());
            }
            localFaceIndexesList.Clear();
            curBatch.MakeBoundingBox();
            curBatch.AddPacket(finalPac);
            curBatch.m_activeAttribs.AddRange(localAttribs);
            localAttribs.Clear();
            m_batches.Add(curBatch);
            m_totalAttribs.Add(AttributeType.NullAttr);

            for (int i = 0; i < m_batches.Count; i++)
            {
                m_batches[i].Material = MatContainer.Materials[i];
            }
        }

        private static void WriteHeader(EndianBinaryWriter writer)
        {
            writer.Write("J3D2bmd3".ToCharArray());
            writer.Write((int)0);
            writer.Write((int)8);
            writer.Write("SVR3".ToCharArray());
            writer.Write((int)-1);
            writer.Write((int)-1);
            writer.Write((int)-1);
        }

        private static void WriteInf1(EndianBinaryWriter writer)
        {
            writer.Write("INF1".ToCharArray());
            writer.Write((int)0);
            writer.Write((short)0);
            writer.Write((short)-1);
            writer.Write((int)m_batches.Count);
            writer.Write((int)m_vertexes.Count);
            writer.Write((int)0x18);

            writer.Write((short)HierarchyDataTypes.Joint);
            writer.Write((short)0);
            writer.Write((short)HierarchyDataTypes.NewNode);
            writer.Write((short)0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((short)HierarchyDataTypes.Joint);
                writer.Write((short)(i + 1));

                writer.Write((short)HierarchyDataTypes.NewNode);
                writer.Write((short)0);

                writer.Write((short)HierarchyDataTypes.Material);
                writer.Write((short)i);

                writer.Write((short)HierarchyDataTypes.NewNode);
                writer.Write((short)0);

                writer.Write((short)HierarchyDataTypes.Shape);
                writer.Write((short)i);

                writer.Write((short)HierarchyDataTypes.EndNode);
                writer.Write((short)0);

                writer.Write((short)HierarchyDataTypes.EndNode);
                writer.Write((short)0);

                writer.Write((short)HierarchyDataTypes.EndNode);
                writer.Write((short)0);

                if (i != m_batches.Count - 1)
                {
                    writer.Write((short)HierarchyDataTypes.NewNode);
                    writer.Write((short)0);
                }
            }

            writer.Write((short)HierarchyDataTypes.Finish);
            writer.Write((short)0);

            Pad32(writer);

            writer.BaseStream.Seek(0x4, 0);
            writer.Write((int)writer.BaseStream.Length);
        }

        private static void WriteVtx1(EndianBinaryWriter writer)
        {
            writer.Write("VTX1".ToCharArray());
            writer.Write((int)0);
            writer.Write((int)0x40);
            writer.Write((int)0);

            for (int i = 0; i < 12; i++)
            {
                writer.Write((int)0);
            }

            foreach (AttributeType attrib in m_totalAttribs)
            {
                if (attrib == AttributeType.Position)
                {
                    writer.Write((int)AttributeType.Position);
                    writer.Write((int)1);
                    writer.Write((int)4);
                    writer.Write((short)0x00FF);
                    writer.Write((short)-1);
                }

                else if (attrib == AttributeType.Tex0)
                {
                    writer.Write((int)AttributeType.Tex0);
                    writer.Write((int)1);
                    writer.Write((int)4);
                    writer.Write((short)0x00FF);
                    writer.Write((short)-1);
                }

                else if (attrib == AttributeType.Normal)
                {
                    writer.Write((int)AttributeType.Normal);
                    writer.Write((int)1);
                    writer.Write((int)4);
                    writer.Write((short)0x00FF);
                    writer.Write((short)-1);
                }

                else if (attrib == AttributeType.NullAttr)
                {
                    writer.Write((int)AttributeType.NullAttr);
                    writer.Write((int)1);
                    writer.Write((int)0);
                    writer.Write((short)0x00FF);
                    writer.Write((short)-1);
                }
            }

            Pad32(writer);

            writer.BaseStream.Seek(0xC, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            foreach (Vector3 vec in m_vertexes)
            {
                writer.Write(vec.X);
                writer.Write(vec.Y);
                writer.Write(vec.Z);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x20, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            if (m_totalAttribs.Contains(AttributeType.Tex0))
            {
                foreach (Vector2 vec in m_texCoords)
                {
                    writer.Write(vec.X);
                    writer.Write(vec.Y);
                }
            }

            Pad32(writer);

            if (m_totalAttribs.Contains(AttributeType.Normal))
            {
                foreach (Vector3 vec in m_normals)
                {
                    writer.Write(vec.X);
                    writer.Write(vec.Y);
                    writer.Write(vec.Z);
                }
            }

            Pad32(writer);

            writer.BaseStream.Seek(4, 0);
            writer.Write((int)writer.BaseStream.Length);
        }

        private static void WriteEvp1(EndianBinaryWriter writer)
        {
            writer.Write("EVP1".ToCharArray());
            writer.Write((int)0x20);
            writer.Write((short)0);
            writer.Write((short)-1);

            for (int i = 0; i < 16; i++)
            {
                writer.Write((byte)0);
            }

            Pad32(writer);
        }

        private static void WriteDrw1(EndianBinaryWriter writer)
        {
            writer.Write("DRW1".ToCharArray());
            writer.Write((int)0);
            writer.Write((short)m_batches.Count);
            writer.Write((short)-1);
            writer.Write(0x14);
            writer.Write(0x0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((byte)0);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x10, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((short)(i + 1));
            }

            Pad32(writer);

            writer.BaseStream.Seek(4, 0);
            writer.Write((int)writer.BaseStream.Length);
        }

        private static void WriteJnt1(EndianBinaryWriter writer)
        {
            writer.Write("JNT1".ToCharArray());
            writer.Write((int)0);
            writer.Write((short)(m_batches.Count + 1));
            writer.Write((short)-1);
            writer.Write((int)0x18);
            writer.Write((int)((m_batches.Count + 1) * 0x40));
            writer.Write((int)0); // Placeholder for offset to the string table

            Bone root = new Bone(new BoundingBox(), "world_root");

            root.WriteBone(writer);

            foreach (Batch bat in m_batches)
                bat.WriteBone(writer);

            writer.BaseStream.Seek(0x10, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count + 1; i++)
            {
                writer.Write((short)i);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x14, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);
            writer.Write((short)(m_batches.Count + 1));
            writer.Write((short)-1);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            short stringOffset = (short)(4 + ((m_batches.Count + 1) * 4));

            writer.Write(HashName("world_root"));
            writer.Write(stringOffset);
            stringOffset += (short)("world_root".Length + 1);

            foreach (Batch bat in m_batches)
            {
                writer.Write(HashName(bat.GetName()));
                writer.Write(stringOffset);
                stringOffset += (short)(bat.GetName().Length + 1);
            }

            writer.Write("world_root".ToCharArray());
            writer.Write((byte)0);

            foreach (Batch bat in m_batches)
            {
                writer.Write(bat.GetName().ToCharArray());
                writer.Write((byte)0);
            }

            Pad32(writer);

            writer.BaseStream.Seek(4, 0);

            writer.Write((int)writer.BaseStream.Length);
        }

        private static void WriteShp1(EndianBinaryWriter writer)
        {
            writer.Write("SHP1".ToCharArray());
            writer.Write((int)0);
            writer.Write((short)m_batches.Count);
            writer.Write((short)-1);
            writer.Write((int)0x2C);

            for (int i = 0; i < 7; i++)
                writer.Write((int)0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                m_batches[i].WriteBatch(writer, i);
            }

            //Pad32(writer);

            writer.BaseStream.Seek(0x10, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((short)i);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x18, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            foreach (AttributeType type in m_totalAttribs)
            {
                writer.Write((int)type);
                writer.Write((int)3);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x1C, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((short)i);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x20, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            List<short> offsets = new List<short>() { 0 };

            short curOffset = (short)writer.BaseStream.Position;

            foreach (Batch bat in m_batches)
            {
                bat.WritePacket(writer);

                offsets.Add((short)(writer.BaseStream.Position - curOffset));
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x24, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                writer.Write((short)0);
                writer.Write((short)1);
                writer.Write((int)i);
            }

            //Pad32(writer);

            writer.BaseStream.Seek(0x28, 0);
            writer.Write((int)writer.BaseStream.Length);

            writer.BaseStream.Seek(writer.BaseStream.Length, 0);

            for (int i = 0; i < m_batches.Count; i++)
            {
                short packetSize = m_batches[i].GetPacketSize(0);

                writer.Write((int)packetSize);
                writer.Write((int)offsets[i]);
            }

            Pad32(writer);

            writer.BaseStream.Seek(0x4, 0);
            writer.Write((int)writer.BaseStream.Length);
        }

        private static void WriteMat3(EndianBinaryWriter writer)
        {
            MatContainer.WriteMat3(writer);
        }

        private static void WriteTex1(EndianBinaryWriter writer)
        {
            // Write TEX1 header
            writer.Write("TEX1".ToCharArray());
            writer.Write((int)0);
            writer.Write((short)m_textures.Count);
            writer.Write((short)-1);
            writer.Write((int)0x20);
            writer.Write((int)0);

            Pad32(writer);

            // Write BTIHeaders
            foreach (BinaryTextureImage tex in m_textures)
                tex.WriteHeader(writer);

            //int imageDataOffset = (int)writer.BaseStream.Length - (m_textures.Count * 0x20);

            // Write image data offsets and image data
            for (int i = 0; i < m_textures.Count; i++)
            {
                int curHeaderOffset = 0x20 + (i * 0x20);
                int imageDataOffset = (int)writer.BaseStream.Length - curHeaderOffset;
                // 0x20 is the TEX1 header size, 0x0C is the offset to paletteDataOffset,
                // i * 0x20 is the current header
                writer.Seek(0x20 + 0x0C + (i * 0x20), 0);
                writer.Write((int)(imageDataOffset - (i* 20)));

                // 0x20 is the TEX1 header size, 0x1C is the offset to the imageDataOffset,
                // i * 0x20 is the current header
                writer.Seek(0x20 + 0x1C + (i * 0x20), 0);
                writer.Write((int)imageDataOffset);

                // Write actual data
                writer.Seek((int)writer.BaseStream.Length, 0);
                //imageDataOffset = (int)writer.BaseStream.Length - 0x20;
                writer.Write(m_textures[i].GetData());
            }

            writer.Seek(0x10, 0);
            writer.Write((int)writer.BaseStream.Length);
            writer.Seek((int)writer.BaseStream.Length, 0);

            // Write string table
            writer.Write((short)m_textures.Count);
            writer.Write((short)-1);

            short stringOffset = (short)(4 + ((m_textures.Count) * 4));

            // Hash and string offset
            foreach (BinaryTextureImage tex in m_textures)
            {
                writer.Write((short)HashName(tex.Name));
                writer.Write(stringOffset);

                stringOffset += (short)(tex.Name.Length + 1);
            }

            // String data with null terminators
            foreach (BinaryTextureImage tex in m_textures)
            {
                writer.Write(tex.Name.ToCharArray());
                writer.Write((byte)0);
            }

            Pad32(writer);

            writer.Seek(4, 0);
            writer.Write((int)writer.BaseStream.Length);
        }

        private static ushort HashName(string name)
        {
            short hash = 0;

            short multiplier = 1;

            if (name.Length + 1 == 2)
            {
                multiplier = 2;
            }

            if (name.Length + 1 >= 3)
            {
                multiplier = 3;
            }

            foreach (char c in name)
            {
                hash = (short)(hash * multiplier);
                hash += (short)c;
            }

            return (ushort)hash;
        }

        private static void Pad32(EndianBinaryWriter writer)
        {
            // Pad up to a 32 byte alignment
            // Formula: (x + (n-1)) & ~(n-1)
            long nextAligned = (writer.BaseStream.Length + 0x1F) & ~0x1F;

            long delta = nextAligned - writer.BaseStream.Length;
            writer.BaseStream.Position = writer.BaseStream.Length;
            for (int i = 0; i < delta; i++)
            {
                writer.Write(padString[i]);
            }
        }

        private static Vector3 ParseVec3(string[] vertLine)
        {
            Vector3 vec = new Vector3();

            for (int i = 1; i < vertLine.Length; i++)
            {
                if (vertLine[i] != "")
                {
                    try
                    {
                        vec.X = Convert.ToSingle(vertLine[i]);
                        vec.Y = Convert.ToSingle(vertLine[i + 1]);
                        vec.Z = Convert.ToSingle(vertLine[i + 2]);

                        return vec;
                    }
                    catch
                    {
                        Console.WriteLine("Couldn't parse vector 3.");
                    }
                }
            }

            return vec;
        }

        private static Vector2 ParseVec2(string[] uvLine)
        {
            Vector2 uv = new Vector2();

            try
            {
                uv.X = Convert.ToSingle(uvLine[1]);
                uv.Y = 1.0f - Convert.ToSingle(uvLine[2]);
            }
            catch
            {
                Console.WriteLine("Couldn't parse vector 2.");
            }

            return uv;
        }
    }
}
