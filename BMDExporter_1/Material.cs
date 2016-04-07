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
        public Color[] MaterialColors;
        public ChannelControl[] ChannelControls;
        public Color[] AmbientColors;
        public Color?[] LightingColors;
        public TexCoordGen[] TexCoord1Gens;
        public TexCoordGen[] TexCoord2Gens;
        public TexMatrix[] TexMatrix1;
        public TexMatrix[] TexMatrix2;
        public BinaryTextureImage[] Textures;
        public TevOrder[] TevOrders;
        public GXKonstColorSel[] ColorSels;
        public GXKonstAlphaSel[] AlphaSels;
        public Color[] TevColors;
        public Color[] KonstColors;
        public TevStage[] TevStages;
        public TevSwapMode[] SwapModes;
        public TevSwapModeTable[] SwapTables;

        public Material()
        {
            Flag = 1;
            IndTexEntry = new IndirectTexturing();
            CullMode = GXCullMode.Back;
            MaterialColors = new Color[2] { new Color(1, 1, 1, 1), new Color(1, 1, 1, 1) };
            ChannelControls = new ChannelControl[4]
            {
                new ChannelControl(false, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0),
                new ChannelControl(true, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)1, (GXAttenuationFn)0, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)0, (GXLightId)0, (GXDiffuseFn)0, (GXAttenuationFn)2, (GXColorSrc)0),
                new ChannelControl(false, (GXColorSrc)1, (GXLightId)0, (GXDiffuseFn)2, (GXAttenuationFn)1, (GXColorSrc)0)
            };
            AmbientColors = new Color[2] { new Color(0.1960f, 0.1960f, 0.1960f, 0.1960f), new Color(0, 0, 0, 0) };
            LightingColors = new Color?[8];
            TexCoord1Gens = new TexCoordGen[8];
            TexCoord2Gens = new TexCoordGen[8];
            TexMatrix1 = new TexMatrix[10];
            TexMatrix2 = new TexMatrix[20];
            Textures = new BinaryTextureImage[8];
            TevOrders = new TevOrder[16];
            ColorSels = new GXKonstColorSel[16];
            AlphaSels = new GXKonstAlphaSel[16];
            TevColors = new Color[4];
            KonstColors = new Color[4];
            TevStages = new TevStage[16];
            SwapModes = new TevSwapMode[16];
            SwapTables = new TevSwapModeTable[4];
        }
    }
}
