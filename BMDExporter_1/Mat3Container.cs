using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace BMDExporter_1
{
    class Mat3Container
    {
        /// <summary>
        /// A list of all the unique indirect texturing entries within the model.
        /// </summary>
        private List<IndirectTexturing> m_indirectTexBlock;
        /// <summary>
        /// A list of all the unique cull modes used by the materials.
        /// </summary>
        private List<GXCullMode> m_cullModeBlock;
        /// <summary>
        /// A list of all the unique material colors used by the materials.
        /// </summary>
        private List<Color> m_materialColorBlock;
        /// <summary>
        /// A list of all the unique channel controls used by the materials.
        /// </summary>
        private List<ChannelControl> m_channelControlBlock;
        /// <summary>
        /// A list of all the unique material colors used by the materials.
        /// </summary>
        private List<Color> m_ambientColorBlock;
        /// <summary>
        /// A list of all the unique material colors used by the materials.
        /// </summary>
        private List<Color?> m_lightingColorBlock;
        /// <summary>
        /// A list of all the unique texture coordinate 1 generation info entries used by the materials.
        /// </summary>
        private List<TexCoordGen> m_texCoord1GenBlock;
        /// <summary>
        /// A list of all the unique texture coordinate 2 generation info entries used by the materials.
        /// </summary>
        private List<TexCoordGen> m_texCoord2GenBlock;
        /// <summary>
        /// A list of all the unique texture matrix 1 entries used by the materials.
        /// </summary>
        private List<TexMatrix> m_texMatrix1Block;
        /// <summary>
        /// A list of all the unique texture matrix 2 entries used by the materials.
        /// </summary>
        private List<TexMatrix> m_texMatrix2Block;
        /// <summary>
        /// A list of all the unique textures used by the materials. Mainly used for getting indexes.
        /// </summary>
        private List<BinaryTextureImage> m_textureBlock;
        /// <summary>
        /// A list of all the unique tev order entries used by the materials.
        /// </summary>
        private List<TevOrder> m_tevOrderBlock;
        /// <summary>
        /// A list of all the unique TEV colors used by the materials.
        /// </summary>
        private List<Color> m_tevColorBlock;
        /// <summary>
        /// A list of all the unique TEV konst colors used by the materials.
        /// </summary>
        private List<Color> m_tevKonstColorBlock;
        /// <summary>
        /// A list of all the unique TEV stage configs used by the materials.
        /// </summary>
        private List<TevStage> m_tevStageBlock;
        /// <summary>
        /// A list of all the unique TEV swap modes used by the materials.
        /// </summary>
        private List<TevSwapMode> m_swapModeBlock;
        /// <summary>
        /// A list of all the unique TEV swap mode tables used by the materials.
        /// </summary>
        private List<TevSwapModeTable> m_swapTableBlock;
        /// <summary>
        /// A list of all the unique fog settings used by the materials.
        /// </summary>
        private List<Fog> m_fogBlock;
        /// <summary>
        /// A list of all the unique alpha compare settings used by the materials.
        /// </summary>
        private List<AlphaCompare> m_alphaCompBlock;
        /// <summary>
        ///  A list of all the unique blend modes used by the materials.
        /// </summary>
        private List<BlendMode> m_blendModeBlock;
        // A list of all the Zmodes used by the materials.
        private List<ZMode> m_zModeBlock;
        // A list of all the unique ZCompLocs used by the materials.
        private List<bool> m_zCompLocBlock;
        // A list of all the unique Dithers used by the materials.
        private List<bool> m_ditherBlock;
        /// <summary>
        /// A list of all the materials within the model.
        /// </summary>
        public List<Material> Materials;

        public Mat3Container()
        {
            m_indirectTexBlock = new List<IndirectTexturing>();
            m_cullModeBlock = new List<GXCullMode>();
            m_materialColorBlock = new List<Color>();
            m_channelControlBlock = new List<ChannelControl>();
            m_ambientColorBlock = new List<Color>();
            m_lightingColorBlock = new List<Color?>();
            m_texCoord1GenBlock = new List<TexCoordGen>();
            m_texCoord2GenBlock = new List<TexCoordGen>();
            m_texMatrix1Block = new List<TexMatrix>();
            m_texMatrix2Block = new List<TexMatrix>();
            m_textureBlock = new List<BinaryTextureImage>();
            m_tevOrderBlock = new List<TevOrder>();
            m_tevColorBlock = new List<Color>();
            m_tevKonstColorBlock = new List<Color>();
            m_tevStageBlock = new List<TevStage>();
            m_swapModeBlock = new List<TevSwapMode>();
            m_swapTableBlock = new List<TevSwapModeTable>();
            m_fogBlock = new List<Fog>();
            m_alphaCompBlock = new List<AlphaCompare>();
            m_blendModeBlock = new List<BlendMode>();
            m_zModeBlock = new List<ZMode>();
            m_zCompLocBlock = new List<bool>();
            m_ditherBlock = new List<bool>();

            Materials = new List<Material>();
        }

        public void WriteMat3(EndianBinaryWriter writer)
        {
            PopulateBlockLists();

            // Write header here

            foreach (Material mat in Materials)
                WriteMaterialIndexes(writer, mat);

            // Write string table here

            // Write indirect texturing offset in header here

            // Write indirect texturing block
            foreach (IndirectTexturing ind in m_indirectTexBlock)
                ind.Write(writer);

            // Write cull mode offset in header here

            // Write cull mode block
            foreach (GXCullMode cull in m_cullModeBlock)
                writer.Write((int)cull);

            // Write material colors offset in header here

            // Write material color block
            foreach (Color col in m_materialColorBlock)
                WriteColor(writer, col);

            // Write number of channel controls offset in header here

            // This is a hack for writing numChanCtrls until more information is collected about it
            writer.Write((byte)1);

            writer.Write("Thi".ToCharArray());

            // Write channel control data offset in header here
            foreach (ChannelControl chan in m_channelControlBlock)
                chan.Write(writer);

            // Write ambient colors offset in header here

            //Write ambient color block
            foreach (Color col in m_ambientColorBlock)
                WriteColor(writer, col);

            // Write lighting colors offset in header here

            // Write lighting color block
            foreach (Color col in m_lightingColorBlock)
                WriteColor(writer, col);

            // Write tex coord 1 gen block
            if (m_texCoord1GenBlock.Count != 0)
            {
                // Write numTexCoord1Gen offset in header here

                // Write numTexCoord1Gen

                // Write TexCoord1Gen offset in header here

                foreach (TexCoordGen gen in m_texCoord1GenBlock)
                    gen.Write(writer);
            }

            // Write tex coord 2 gen block
            if (m_texCoord2GenBlock.Count != 0)
            {
                // Write numTexCoord2Gen offset in header here

                // Write numTexCoord2Gen

                // Write TexCoord2Gen offset in header here

                foreach (TexCoordGen gen in m_texCoord2GenBlock)
                    gen.Write(writer);
            }

            if (m_texMatrix1Block.Count != 0)
            {
                // Write tex matrix 1 offset in header here

                foreach (TexMatrix mat in m_texMatrix1Block)
                    mat.Write(writer);
            }

            if (m_texMatrix1Block.Count != 0)
            {
                // Write tex matrix 2 offset in header here

                foreach (TexMatrix mat in m_texMatrix2Block)
                    mat.Write(writer);
            }

            // Write texture index offset in header here

            for (int i = 0; i < m_textureBlock.Count; i++)
                writer.Write((short)i);

            // Write tev order offset in header here

            foreach (TevOrder order in m_tevOrderBlock)
                order.Write(writer);

            // Write tev color offset in header here

            foreach (Color col in m_tevColorBlock)
                WriteColor(writer, col);

            // Write tev konst color offset in header here

            foreach (Color col in m_tevKonstColorBlock)
                WriteColor(writer, col);
            
            // Write num tev stage offset in header here
            // Write num tev stage here

            // Write tev stage offset in header here

            foreach (TevStage stage in m_tevStageBlock)
                stage.Write(writer);

            // Write swap mode offset in header here

            foreach (TevSwapMode mode in m_swapModeBlock)
                mode.Write(writer);

            // Write swap table offset in header here

            foreach (TevSwapModeTable table in m_swapTableBlock)
                table.Write(writer);

            // Write fog offset in header here

            foreach (Fog fg in m_fogBlock)
                fg.Write(writer);

            // Write alpha compare offset in header here

            foreach (AlphaCompare alph in m_alphaCompBlock)
                alph.Write(writer);

            // Write blend mode offset in header here

            foreach (BlendMode mode in m_blendModeBlock)
                mode.Write(writer);

            // Write z mode offset in header here

            foreach (ZMode mode in m_zModeBlock)
                mode.Write(writer);

            // Write z comp loc offset in header here

            foreach (bool bol in m_zCompLocBlock)
                writer.Write(bol);

            // Write dither offset in header here

            foreach (bool bol in m_ditherBlock)
                writer.Write(bol);
        }

        /// <summary>
        /// Pulls all of the unique fields from each material into lists for easy indexing and output.
        /// </summary>
        private void PopulateBlockLists()
        {
            foreach (Material mat in Materials)
            {
                if (!m_indirectTexBlock.Contains(mat.IndTexEntry))
                    m_indirectTexBlock.Add(mat.IndTexEntry);
                if (!m_cullModeBlock.Contains(mat.CullMode))
                    m_cullModeBlock.Add(mat.CullMode);
                // Material colors
                for (int i = 0; i < 2; i++)
                {
                    if (!m_materialColorBlock.Contains(mat.MaterialColors[i]))
                        m_materialColorBlock.Add(mat.MaterialColors[i]);
                }
                // Channel controls
                for (int i = 0; i < 4; i++)
                {
                    if (!m_channelControlBlock.Contains(mat.ChannelControls[i]))
                        m_channelControlBlock.Add(mat.ChannelControls[i]);
                }
                // Ambient colors
                for (int i = 0; i < 2; i++)
                {
                    if (!m_ambientColorBlock.Contains(mat.AmbientColors[i]))
                        m_ambientColorBlock.Add(mat.AmbientColors[i]);
                }
                // Lighting colors
                for (int lit = 0; lit < 8; lit++)
                {
                    if ((mat.LightingColors[lit] != null) && (!m_lightingColorBlock.Contains(mat.LightingColors[lit])))
                        m_lightingColorBlock.Add(mat.LightingColors[lit]);
                }
                // Tex coord gens 1
                for (int gen = 0; gen < 8; gen++)
                {
                    if ((mat.TexCoord1Gens[gen] != null) && (!m_texCoord1GenBlock.Contains(mat.TexCoord1Gens[gen])))
                        m_texCoord1GenBlock.Add(mat.TexCoord1Gens[gen]);
                }
                // Tex coord gens 2
                for (int gen = 0; gen < 8; gen++)
                {
                    if ((mat.TexCoord2Gens[gen] != null) && (!m_texCoord2GenBlock.Contains(mat.TexCoord2Gens[gen])))
                        m_texCoord2GenBlock.Add(mat.TexCoord2Gens[gen]);
                }
                // Texture matrices 1
                for (int tex = 0; tex < 8; tex++)
                {
                    if ((mat.TexMatrix1[tex] != null) && (!m_texMatrix1Block.Contains(mat.TexMatrix1[tex])))
                        m_texMatrix1Block.Add(mat.TexMatrix1[tex]);
                }
                // Texture matrices 2
                for (int tex = 0; tex < 22; tex++)
                {
                    if ((mat.TexMatrix2[tex] != null) && (!m_texMatrix2Block.Contains(mat.TexMatrix2[tex])))
                        m_texMatrix2Block.Add(mat.TexMatrix2[tex]);
                }
                // Textures
                for (int text = 0; text < 8; text++)
                {
                    if ((mat.Textures[text] != null) && (!m_textureBlock.Contains(mat.Textures[text])))
                        m_textureBlock.Add(mat.Textures[text]);
                }
                // Tev orders
                for (int i = 0; i < 16; i++)
                {
                    if ((mat.TevOrders[i] != null) && (!m_tevOrderBlock.Contains(mat.TevOrders[i])))
                        m_tevOrderBlock.Add(mat.TevOrders[i]);
                }
                // Tev colors
                for (int i = 0; i < 4; i++)
                {
                    if (m_tevColorBlock.Contains(mat.TevColors[i]))
                        m_tevColorBlock.Add(mat.TevColors[i]);
                }
                // Tev konst colors
                for (int i = 0; i < 4; i++)
                {
                    if (m_tevKonstColorBlock.Contains(mat.KonstColors[i]))
                        m_tevKonstColorBlock.Add(mat.KonstColors[i]);
                }
                // Tev stages
                for (int i = 0; i < 16; i++)
                {
                    if ((mat.TevStages[i] != null) && (!m_tevStageBlock.Contains(mat.TevStages[i])))
                        m_tevStageBlock.Add(mat.TevStages[i]);
                }
                // Tev swap modes
                for (int i = 0; i < 16; i++)
                {
                    if ((mat.SwapModes[i] != null) && (!m_swapModeBlock.Contains(mat.SwapModes[i])))
                        m_swapModeBlock.Add(mat.SwapModes[i]);
                }
                // Tev swap tables
                for (int i = 0; i < 4; i++)
                {
                    if ((mat.SwapTables[i] != null) && (!m_swapTableBlock.Contains(mat.SwapTables[i])))
                        m_swapTableBlock.Add(mat.SwapTables[i]);
                }
                // Fog
                if (!m_fogBlock.Contains(mat.FogInfo))
                    m_fogBlock.Add(mat.FogInfo);
                // Alpha compare
                if (!m_alphaCompBlock.Contains(mat.AlphCompare))
                    m_alphaCompBlock.Add(mat.AlphCompare);
                // Blend mode
                if (!m_blendModeBlock.Contains(mat.BMode))
                    m_blendModeBlock.Add(mat.BMode);
                // Z mode
                if (!m_zModeBlock.Contains(mat.ZMode))
                    m_zModeBlock.Add(mat.ZMode);
                // Z comp loc
                if (!m_zCompLocBlock.Contains(mat.ZCompLoc))
                    m_zCompLocBlock.Add(mat.ZCompLoc);
                // Dither
                if (!m_ditherBlock.Contains(mat.Dither))
                    m_ditherBlock.Add(mat.Dither);
            }
        }

        // Outputs the specified color to the specified stream.
        private void WriteColor(EndianBinaryWriter writer, Color col)
        {
            writer.Write((byte)(col.R * 255f));
            writer.Write((byte)(col.G * 255f));
            writer.Write((byte)(col.B * 255f));
            writer.Write((byte)(col.A * 255f));
        }
        private void WriteMaterialIndexes(EndianBinaryWriter writer, Material mat)
        {
            // Flag
            writer.Write(mat.Flag);
            // Cull mode
            writer.Write((byte)m_cullModeBlock.IndexOf(mat.CullMode));
            // NumChannelControls
            // NumTexGens
            // NumTevStages
            // ZCompLoc
            writer.Write((byte)m_zCompLocBlock.IndexOf(mat.ZCompLoc));
            // ZMode
            writer.Write((byte)m_zModeBlock.IndexOf(mat.ZMode));
            // Dither
            writer.Write((byte)m_ditherBlock.IndexOf(mat.Dither));
            // Material color
            for (int i = 0; i < 2; i++)
            {
                writer.Write((short)m_materialColorBlock.IndexOf(mat.MaterialColors[0]));
            }
            // Channel controls
            for (int i = 0; i < 4; i++)
            {
                writer.Write((short)m_channelControlBlock.IndexOf(mat.ChannelControls[i]));
            }
            // Ambient color
            for (int i = 0; i < 2; i++)
            {
                writer.Write((short)m_ambientColorBlock.IndexOf(mat.AmbientColors[i]));
            }
            // Lighting color
            for (int i = 0; i < 8; i++)
            {
                if (mat.LightingColors[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_lightingColorBlock.IndexOf(mat.LightingColors[i]));
            }
            // Tex coord gens 1
            for (int i = 0; i < 8; i++)
            {
                if (mat.TexCoord1Gens[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_texCoord1GenBlock.IndexOf(mat.TexCoord1Gens[i]));
            }
            // Tex coord gens 2
            for (int i = 0; i < 8; i++)
            {
                if (mat.TexCoord2Gens[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_texCoord2GenBlock.IndexOf(mat.TexCoord2Gens[i]));
            }
            // Tex matrix 1
            for (int i = 0; i < 10; i++)
            {
                if (mat.TexMatrix1[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_texMatrix1Block.IndexOf(mat.TexMatrix1[i]));
            }
            // Tex matrix 2
            for (int i = 0; i < 20; i++)
            {
                if (mat.TexMatrix2[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_texMatrix2Block.IndexOf(mat.TexMatrix2[i]));
            }
            // Texture indices
            for (int i = 0; i < 8; i++)
            {
                if (mat.Textures[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_textureBlock.IndexOf(mat.Textures[i]));
            }
            // TevKonstColors
            for (int i = 0; i < 4; i++)
            {
                if (mat.KonstColors[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_tevKonstColorBlock.IndexOf(mat.KonstColors[i]));
            }
            // Konst Color Sels
            for (int i = 0; i < 16; i++)
            {
                writer.Write((byte)mat.ColorSels[i]);
            }
            // Konst Alpha Sels
            for (int i = 0; i < 16; i++)
            {
                writer.Write((byte)mat.AlphaSels[i]);
            }
            // Tev order info
            for (int i = 0; i < 16; i++)
            {
                if (mat.TevOrders[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_tevOrderBlock.IndexOf(mat.TevOrders[i]));
            }
            // Tev Color
            for (int i = 0; i < 4; i++)
            {
                if (mat.TevColors[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_tevColorBlock.IndexOf(mat.TevColors[i]));
            }
            // Tev Stage Info
            for (int i = 0; i < 16; i++)
            {
                if (mat.TevStages[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_tevStageBlock.IndexOf(mat.TevStages[i]));
            }
            // TevSwapModes
            for (int i = 0; i < 16; i++)
            {
                if (mat.SwapModes[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_swapModeBlock.IndexOf(mat.SwapModes[i]));
            }
            // TevSwapModeTables
            for (int i = 0; i < 4; i++)
            {
                if (mat.SwapTables[i] == null)
                    writer.Write((short)-1);
                else
                    writer.Write((short)m_swapTableBlock.IndexOf(mat.SwapTables[i]));
            }
            // Unknown indexes
            for (int i = 0; i < 12; i++)
            {
                writer.Write((short)0);
            }
            // FogIndex
            writer.Write((short)m_fogBlock.IndexOf(mat.FogInfo));
            // AlphaCompare
            writer.Write((short)m_alphaCompBlock.IndexOf(mat.AlphCompare));
            // BlendMode
            writer.Write((short)m_blendModeBlock.IndexOf(mat.BMode));
            // Unknown1
            writer.Write((short)0);
        }
    }
}
