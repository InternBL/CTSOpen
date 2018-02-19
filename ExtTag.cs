using System;
using System.Collections.Generic;
using CTSOpen.NX;
using CTSOpen.NX.AdvancedObjects;
using CTSOpen.NX.Features;
using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;
using Arc = CTSOpen.NX.Arc;
using Body = CTSOpen.NX.Body;
using DatumAxis = CTSOpen.NX.DatumAxis;
using DatumPlane = CTSOpen.NX.DatumPlane;
using Edge = CTSOpen.NX.Edge;
using Ellipse = CTSOpen.NX.AdvancedObjects.Ellipse;
using Face = CTSOpen.NX.Face;
using Line = CTSOpen.NX.Line;
using NXObject = CTSOpen.NX.NXObject;
using Parabola = CTSOpen.NX.AdvancedObjects.Parabola;
using Point = CTSOpen.NX.Point;
using ReferenceSet = CTSOpen.NX.ReferenceSet;

namespace CTSOpen.Extensions
{
    public static class ExtTag
    {
        [Pure]
        public static int Type(this NXOpen.Tag tag)
        {
            int type;
            int subtype;
            UFSession.GetUFSession().Obj.AskTypeAndSubtype(tag, out type, out subtype);
            return type;
        }

        [Pure]
        public static int Subtype(this NXOpen.Tag tag)
        {
            int type;
            int subtype;
            UFSession.GetUFSession().Obj.AskTypeAndSubtype(tag, out type, out subtype);
            return subtype;
        }

        [Pure, NotNull, ItemNotNull, ContractAnnotation("null=>halt")]
        public static IEnumerable<NXObject> ToCtsOpen([NotNull] this IEnumerable<Tag> sourceTags)
        {
            if (sourceTags == null) throw new ArgumentNullException("sourceTags");
            //Debugger.Launch();
            foreach (var tag in sourceTags)
            {
                yield return tag.ToCtsOpen();
            }
        }

        [Pure, NotNull, ContractAnnotation("null=>halt")]
        public static NXObject ToCtsOpen(this Tag tag)
        {
           return NXObject.CreateObject(tag);
        }
    }
}