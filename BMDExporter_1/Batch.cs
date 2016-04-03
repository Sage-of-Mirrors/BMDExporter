using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    public enum AttributeType
    {
        PositionMatrixIndex = 0x00,
        Tex0MatrixIndex = 0x01,
        Tex1MatrixIndex = 0x02,
        Tex2MatrixIndex = 0x03,
        Tex3MatrixIndex = 0x04,
        Tex4MatrixIndex = 0x05,
        Tex5MatrixIndex = 0x06,
        Tex6MatrixIndex = 0x07,
        Tex7MatrixIndex = 0x08,
        Position = 0x09,
        Normal = 0x0A,
        Color0 = 0x0B,
        Color1 = 0x0C,
        Tex0 = 0x0D,
        Tex1 = 0x0E,
        Tex2 = 0x0F,
        Tex3 = 0x10,
        Tex4 = 0x11,
        Tex5 = 0x12,
        Tex6 = 0x13,
        Tex7 = 0x14,
        PositionMatrixArray = 0x15,
        NormalMatrixArray = 0x16,
        TextureMatrixArray = 0x17,
        LitMatrixArray = 0x18,
        NormalBinormalTangent = 0x19,
        MaxAttr = 0x1A,
        NullAttr = 0xFF,
    }

    class Batch
    {
        string m_name;
        Bone m_bone;
        Material m_material;
        BoundingBox m_boundingBox;

        List<Packet> m_packets;
        List<AttributeType> m_activeAttribs;

        public Batch()
        {
            m_packets = new List<Packet>();
            m_activeAttribs = new List<AttributeType>() { AttributeType.Position };
        }

        public void AddPacket(Packet pack)
        {
            m_packets.Add(pack);
        }

        public bool HasAttribute(AttributeType attrib)
        {
            return m_activeAttribs.Contains(attrib);
        }

        public void AddAttribute(AttributeType attrib)
        {
            m_activeAttribs.Add(attrib);
        }

        public void SetName(string name)
        {
            m_name = name;
        }

        public string GetName()
        {
            return m_name;
        }

        public void SetBoundingBox(BoundingBox box)
        {
            m_boundingBox = box;
        }

        public void SetMaterial(Material mat)
        {
            m_material = mat;
        }

        public void CreateBone()
        {
            m_bone = new Bone(m_boundingBox, m_name);
        }

        public void WriteBone(EndianBinaryWriter writer)
        {
            m_bone.WriteBone(writer);
        }

        public void WritePacket(EndianBinaryWriter writer)
        {
            foreach (Packet pack in m_packets)
                pack.WritePacket(writer);
        }

        public void WriteBatch(EndianBinaryWriter writer, int index)
        {
            writer.Write((short)0x00FF);
            writer.Write((short)(m_packets.Count));
            writer.Write((int)index);
            writer.Write((short)index);
            writer.Write((short)-1);

            writer.Write(m_boundingBox.SphereRadius);

            writer.Write(m_boundingBox.Minimum.X);
            writer.Write(m_boundingBox.Minimum.Y);
            writer.Write(m_boundingBox.Minimum.Z);

            writer.Write(m_boundingBox.Maximum.X);
            writer.Write(m_boundingBox.Maximum.Y);
            writer.Write(m_boundingBox.Maximum.Z);
        }

        public void WritePacketSize(EndianBinaryWriter writer)
        {
            foreach (Packet pck in m_packets)
                pck.WriteSize(writer);
        }

        public short GetPacketSize(int index)
        {
            return m_packets[index].GetSize();
        }
    }
}
