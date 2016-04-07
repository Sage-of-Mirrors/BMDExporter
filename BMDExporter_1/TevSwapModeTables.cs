using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class TevSwapModeTable
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public TevSwapModeTable()
        {

        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
            writer.Write(A);
        }
    }
}
