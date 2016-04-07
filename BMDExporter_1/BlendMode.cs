using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class BlendMode
    {
        /// <summary> Blending Type </summary>
        public GXBlendMode Type;

        /// <summary> Blending Control </summary>
        public GXBlendModeControl SourceFact;

        /// <summary> Blending Control </summary>
        public GXBlendModeControl DestinationFact;

        /// <summary> What operation is used to blend them when <see cref="Type"/> is set to <see cref="GXBlendMode.Logic"/>. </summary>
        public GXLogicOp Operation; // Seems to be logic operators such as clear, and, copy, equiv, inv, invand, etc.

        public BlendMode()
        {

        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write((byte)SourceFact);
            writer.Write((byte)DestinationFact);
            writer.Write((byte)Operation);
        }
    }
}
