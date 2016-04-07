using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class TevStage
    {
        public byte Unknown0; // Always 0xFF
        public GXCombineColorInput[] ColorIn; // 4
        public GXTevOp ColorOp;
        public GXTevBias ColorBias;
        public GXTevScale ColorScale;
        public bool ColorClamp;
        public byte ColorRegId;
        public GXCombineAlphaInput[] AlphaIn; // 4
        public GXTevOp AlphaOp;
        public GXTevBias AlphaBias;
        public GXTevScale AlphaScale;
        public bool AlphaClamp;
        public byte AlphaRegId;
        public byte Unknown1; // Always 0xFF

        public TevStage()
        {
            ColorIn = new GXCombineColorInput[4];
            AlphaIn = new GXCombineAlphaInput[4];
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Unknown0);
            for (int i = 0; i < 4; i++)
                writer.Write((byte)ColorIn[i]);
            writer.Write((byte)ColorOp);
            writer.Write((byte)ColorBias);
            writer.Write((byte)ColorScale);
            writer.Write(ColorClamp);
            writer.Write(ColorRegId);
            for (int i = 0; i < 4; i++)
                writer.Write((byte)AlphaIn[i]);
            writer.Write((byte)AlphaOp);
            writer.Write((byte)AlphaBias);
            writer.Write(AlphaClamp);
            writer.Write(AlphaRegId);
            writer.Write(Unknown1);

            // Pad to ? bytes
            writer.Write((short)-1);
        }
    }
}
