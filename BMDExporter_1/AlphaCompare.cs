using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class AlphaCompare
    {
        /// <summary> subfunction 0 </summary>
        public GXCompareType Comp0;
        /// <summary> Reference value for subfunction 0. </summary>
        public byte Reference0;
        /// <summary> Alpha combine control for subfunctions 0 and 1. </summary>
        public GXAlphaOp Operation;
        /// <summary> subfunction 1 </summary>
        public GXCompareType Comp1;
        /// <summary> Reference value for subfunction 1. </summary>
        public byte Reference1;

        public AlphaCompare()
        {

        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte)Comp0);
            writer.Write(Reference0);
            writer.Write((byte)Operation);
            writer.Write((byte)Comp1);
            writer.Write(Reference1);
            // Pad entry to 8 bytes
            writer.Write((byte)0xFF);
            writer.Write((byte)0xFF);
            writer.Write((byte)0xFF);
        }
    }
}
