using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDExporter_1
{
    class Material
    {
        /*0x000*/ byte unknown1; //Read by patched material, always 1?
        /*0x001*/ byte unknown2; //Mostly 0, sometimes 2
        /*0x002*/ short padding1; //Always 0?
        /*0x004*/ short indirectTexturingIndex;
        /*0x006*/ short cullModeIndex;

        public Material()
        {
            unknown1 = 1;
            unknown2 = 0;
            padding1 = 0;
        }

        private void CreateTestMaterial()
        {

        }
    }
}
