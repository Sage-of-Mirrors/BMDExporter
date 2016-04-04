using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BMDExporter_1
{
    class Material
    {
        /*0x000*/ byte unknown1; //Read by patched material, always 1?
        /*0x001*/ byte unknown2; //Mostly 0, sometimes 2
        /*0x002*/ short padding1; //Always 0?
        /*0x004*/ short indirectTexturingIndex;
        /*0x006*/ short cullModeIndex;

        public string Name { get; private set; }

        public BinaryTextureImage Texture { get; private set; }

        public Material()
        {
            unknown1 = 1;
            unknown2 = 0;
            padding1 = 0;
        }

        public Material(string name, StreamReader source)
        {
            Name = name;

            while (!source.EndOfStream)
            {
                string line = source.ReadLine();
                string[] decompLine = line.Split(' ');

                switch (decompLine[0])
                {
                    case "newmtl":
                        if (decompLine[1] == Name)
                            CreateMaterial(source);
                        break;
                }
            }

            source.BaseStream.Position = 0;
        }
        
        private void CreateMaterial(StreamReader source)
        {
            string line = "";
            string[] decompLine = line.Split(' ');

            while ((decompLine[0] != "newmtl") && (!source.EndOfStream))
            {
                line = source.ReadLine();
                decompLine = line.Split(' ');

                switch(decompLine[0])
                {
                    case "map_Kd":
                        GetTexture(line, source);
                        break;
                }
            }
        }

        private void GetTexture(string fileNameSource, StreamReader source)
        {
            Texture = new BinaryTextureImage("test", new Bitmap(Path.GetDirectoryName(((FileStream)source.BaseStream).Name) + @"\" + fileNameSource.Substring(7)));
        }

        private void CreateTestMaterial()
        {
            
        }
    }
}
