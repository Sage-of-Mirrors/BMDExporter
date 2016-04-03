using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using OpenTK;

namespace BMDExporter_1
{
    class Bone
    {
        string m_name;

        Vector3 m_translation;
        Vector3 m_scale;
        Vector3 m_rotation;

        BoundingBox m_boundingBox;

        public Bone(BoundingBox bound, string name)
        {
            m_boundingBox = bound;
            m_name = name;
            m_translation = new Vector3();
            m_scale = Vector3.One;
            m_rotation = new Vector3();
        }

        public void WriteBone(EndianBinaryWriter writer)
        {
            writer.Write((short)0);
            writer.Write((short)0x00FF);

            writer.Write(m_scale.X);
            writer.Write(m_scale.Y);
            writer.Write(m_scale.Z);

            writer.Write((short)0);
            writer.Write((short)0);
            writer.Write((short)0);
            writer.Write((short)-1);

            writer.Write(m_translation.X);
            writer.Write(m_translation.Y);
            writer.Write(m_translation.Z);

            writer.Write(m_boundingBox.SphereRadius);

            writer.Write(m_boundingBox.Minimum.X);
            writer.Write(m_boundingBox.Minimum.Y);
            writer.Write(m_boundingBox.Minimum.Z);

            writer.Write(m_boundingBox.Maximum.X);
            writer.Write(m_boundingBox.Maximum.Y);
            writer.Write(m_boundingBox.Maximum.Z);
        }
    }
}
