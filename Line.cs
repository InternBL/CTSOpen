using System;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public class Line : DisplayableObject
    {
        public static bool IsLine(NXOpen.Tag tag)
        {
            int type;
            int subtype;
            UFSession.GetUFSession().Obj.AskTypeAndSubtype(tag,out type, out subtype);

            return type == UFConstants.UF_line_type;
        }



        public Line(Tag tag) : base(tag)
        {
           if(!IsLine(tag))
                throw new InvalidOperationException("Wasn't a line.");




        }
    }
}
