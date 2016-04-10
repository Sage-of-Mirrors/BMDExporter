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
        public string Name;
        public byte Flag;
        public IndirectTexturing IndTexEntry;
        public GXCullMode CullMode;
        public Color?[] MaterialColors;
        public ChannelControl[] ChannelControls;
        public Color?[] AmbientColors;
        public Color?[] LightingColors;
        public TexCoordGen[] TexCoord1Gens;
        public TexCoordGen[] TexCoord2Gens;
        public TexMatrix[] TexMatrix1;
        public TexMatrix[] TexMatrix2;
        public BinaryTextureImage[] Textures;
        public TevOrder[] TevOrders;
        public GXKonstColorSel[] ColorSels;
        public GXKonstAlphaSel[] AlphaSels;
        public Color?[] TevColors;
        public Color?[] KonstColors;
        public TevStage[] TevStages;
        public TevSwapMode[] SwapModes;
        public TevSwapModeTable[] SwapTables;
        public Fog FogInfo;
        public AlphaCompare AlphCompare;
        public BlendMode BMode;
        public ZMode ZMode;
        public bool ZCompLoc;
        public bool Dither;

        public Material()
        {
            Flag = 1;
            IndTexEntry = new IndirectTexturing();
            CullMode = GXCullMode.Back;
            MaterialColors = new Color?[2] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) };
            ChannelControls = new ChannelControl[4]
            {
                new ChannelControl(false, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0),
                new ChannelControl(true, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)1, (GXAttenuationFn)0, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)0, (GXAttenuationFn)2, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)1, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0)
            };
            AmbientColors = new Color?[2] { new Color(0.1960f, 0.1960f, 0.1960f, 0.1960f), new Color(0, 0, 0, 0) };
            LightingColors = new Color?[8];
            TexCoord1Gens = new TexCoordGen[8];
            TexCoord2Gens = new TexCoordGen[8];
            TexMatrix1 = new TexMatrix[10];
            TexMatrix2 = new TexMatrix[20];
            Textures = new BinaryTextureImage[8];
            TevOrders = new TevOrder[16];
            ColorSels = new GXKonstColorSel[16];
            AlphaSels = new GXKonstAlphaSel[16];
            for (int i = 0; i < 16; i++)
            {
                ColorSels[i] = (GXKonstColorSel)0x0C;
                AlphaSels[i] = (GXKonstAlphaSel)0x1C;
            }
            TevColors = new Color?[4] { new Color(), new Color(), new Color(), new Color() };
            KonstColors = new Color?[4] { new Color(), new Color(), new Color(), new Color() };
            TevStages = new TevStage[16];
            TevStages[0] = new TevStage();
            SwapModes = new TevSwapMode[16];
            SwapModes[0] = new TevSwapMode();
            SwapTables = new TevSwapModeTable[4] { new TevSwapModeTable(), new TevSwapModeTable(), new TevSwapModeTable(), new TevSwapModeTable() };
            FogInfo = new Fog();
            AlphCompare = new AlphaCompare();
            BMode = new BlendMode();
            ZMode = new ZMode();
        }

        public Material(string name, StreamReader source, string sourceFolder)
        {
            Name = name;
            Flag = 1;
            IndTexEntry = new IndirectTexturing();
            CullMode = GXCullMode.Back;
            MaterialColors = new Color?[2] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) };
            ChannelControls = new ChannelControl[4]
            {
                new ChannelControl(false, (GXColorSrc)1, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)1, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0),
                new ChannelControl(true, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)1, (GXAttenuationFn)0, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)0, (GXAttenuationFn)2, (GXColorSrc)0),
                //new ChannelControl(true, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)0, (GXAttenuationFn)2, (GXColorSrc)0),
                //new ChannelControl(false, (GXColorSrc)1, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0)
            };
            AmbientColors = new Color?[2] { new Color(0.1960f, 0.1960f, 0.1960f, 0.1960f), new Color(0, 0, 0, 0) };
            LightingColors = new Color?[8];
            TexCoord1Gens = new TexCoordGen[8];
            TexCoord2Gens = new TexCoordGen[8];
            TexMatrix1 = new TexMatrix[10];
            TexMatrix2 = new TexMatrix[20];
            Textures = new BinaryTextureImage[8];
            TevOrders = new TevOrder[16];
            TevOrders[0] = new TevOrder((GXTexCoordSlot)0, 0, (GXColorChannelId)4);
            TevOrders[1] = new TevOrder((GXTexCoordSlot)0, 0, (GXColorChannelId)0xFF);
            ColorSels = new GXKonstColorSel[16];
            AlphaSels = new GXKonstAlphaSel[16];
            for (int i = 0; i < 16; i++)
            {
                ColorSels[i] = (GXKonstColorSel)0x0C;
                AlphaSels[i] = (GXKonstAlphaSel)0x1C;
            }
            TevColors = new Color?[4] { new Color(0, 0, 0, 1), new Color(1, 1, 1, 1), new Color(0, 0, 0, 0), new Color(1, 1, 1, 1) };
            KonstColors = new Color?[4] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1), new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) };
            TevStages = new TevStage[16];
            TevStages[0] = new TevStage(new GXCombineColorInput[] { (GXCombineColorInput)2, (GXCombineColorInput)0xe, (GXCombineColorInput)0xA, (GXCombineColorInput)0xF},
                (GXTevOp)0, (GXTevBias)0, (GXTevScale)0, true, 0, new GXCombineAlphaInput[] { (GXCombineAlphaInput)7, (GXCombineAlphaInput)4, (GXCombineAlphaInput)5, (GXCombineAlphaInput)7, }, 
                (GXTevOp)0, (GXTevBias)0, (GXTevScale)0, true, 0);
            TevStages[1] = new TevStage(new GXCombineColorInput[] { (GXCombineColorInput)0xF, (GXCombineColorInput)0x8, (GXCombineColorInput)0x0, (GXCombineColorInput)0xF },
                (GXTevOp)0, (GXTevBias)0, (GXTevScale)0, true, 0, new GXCombineAlphaInput[] { (GXCombineAlphaInput)7, (GXCombineAlphaInput)6, (GXCombineAlphaInput)0, (GXCombineAlphaInput)7, },
                (GXTevOp)0, (GXTevBias)0, (GXTevScale)0, true, 0);
            SwapModes = new TevSwapMode[16];
            SwapModes[0] = new TevSwapMode();
            SwapModes[1] = new TevSwapMode();
            SwapTables = new TevSwapModeTable[4] { new TevSwapModeTable(0, 1, 2, 3), new TevSwapModeTable(0, 1, 2, 3), new TevSwapModeTable(0, 1, 2, 3), new TevSwapModeTable(0, 1, 2, 3) };
            FogInfo = new Fog();
            AlphCompare = new AlphaCompare((GXCompareType)7, 0, (GXAlphaOp)1, (GXCompareType)7, 0);
            BMode = new BlendMode((GXBlendMode)0, (GXBlendModeControl)1, (GXBlendModeControl)0, (GXLogicOp)3);
            ZMode = new ZMode(true, (GXCompareType)3, true);

            ZCompLoc = true;
            Dither = true;

            while (!source.EndOfStream)
            {
                string line = source.ReadLine();
                string[] decompLine = line.Split(' ');

                switch (decompLine[0])
                {
                    case "newmtl":
                        if (decompLine[1] == Name)
                        {
                            LoadMaterialData(source, sourceFolder);
                        }
                        break;
                }
            }

            source.BaseStream.Position = 0;
        }

        private void LoadMaterialData(StreamReader source, string sourceFolder)
        {
            while (!source.EndOfStream)
            {
                string line = source.ReadLine();
                string[] decomp = line.Split(' ');

                switch (decomp[0])
                {
                    case "Ka":
                        AmbientColors[0] = new Color(Convert.ToSingle(decomp[1]), Convert.ToSingle(decomp[2]), Convert.ToSingle(decomp[3]), 1);
                        break;
                    case "Kd":
                        MaterialColors[0] = new Color(Convert.ToSingle(decomp[1]), Convert.ToSingle(decomp[2]), Convert.ToSingle(decomp[3]), 1);
                        break;
                    case "map_Kd":
                        if (System.IO.Path.IsPathRooted(decomp[1]))
                        {
                            string texPath = line.Substring(7);
                            string[] test = texPath.Split('\\');
                            Bitmap bmp = new Bitmap(texPath);
                            SetTexture(new BinaryTextureImage(test[test.Length - 1], bmp));
                        }
                        else
                        {
                            string fullTexPath = sourceFolder + @"\" + decomp[1];
                            Bitmap bmp = new Bitmap(fullTexPath);
                            SetTexture(new BinaryTextureImage(decomp[1], bmp));
                        }
                        break;
                    case "newmtl":
                        return;
                }
            }
        }

        public void SetTexture(BinaryTextureImage tex)
        {
            // Search for an open texture slot and if there is one, put the texture there
            for (int i = 0; i < 8; i++)
            {
                if (Textures[i] == null)
                {
                    Textures[i] = tex;
                    break;
                }
            }

            TexCoord1Gens[0] = new TexCoordGen((GXTexGenType)1, (GXTexGenSrc)4, (GXTexMatrix)0x1e);
            TexCoord1Gens[1] = new TexCoordGen((GXTexGenType)1, (GXTexGenSrc)4, (GXTexMatrix)0x3C);
            TexMatrix1[0] = new TexMatrix();
        }
    }
}
