using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class TevSwapMode
    {
        public byte RasSel;
        public byte TexSel;

        public TevSwapMode()
        {

        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(RasSel);
            writer.Write(TexSel);
            // Pad to 4 bytes
            writer.Write((short)-1);
        }
    }
}
