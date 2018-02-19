using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTSOpen.NX;
using JetBrains.Annotations;
using NXOpen.UF;

namespace CTSOpen
{
    public class Globals
    {
        [NotNull]
        public static UFSession UFSession
        {
            get
            {
                var temp = UFSession.GetUFSession();
                if(temp == null)throw new InvalidOperationException("UFSession could not be loaded.");
                return temp;
            }
        }

        [NotNull]
        public static Part DisplayPart
        {
            get
            {
                return new Part(Snap.Globals.DisplayPart.NXOpenTag);
            }
        }



    }
}
