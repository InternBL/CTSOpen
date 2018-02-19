using System;
using System.ComponentModel;
using CTSOpen.Extensions;
using CTSOpen.NX.AdvancedObjects;
using CTSOpen.NX.Features;
using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;
using Ellipse = CTSOpen.NX.AdvancedObjects.Ellipse;
using Parabola = CTSOpen.NX.AdvancedObjects.Parabola;

namespace CTSOpen.NX
{
    public /* abstract*/ class NXObject //: Snap.NX.NXObject
    {

        public static implicit operator NXOpen.Tag(NXObject nxObject) { return nxObject._tag; }

        private readonly Tag _tag;

        public Tag Tag { get { return _tag; } }

        public NXObject(NXOpen.Tag tag) { _tag = tag; }

        public bool IsOccurrence { get { return UFSession.GetUFSession().Assem.IsOccurrence(this); } }

        public string JournalIdentifier { get { return ((NXOpen.NXObject) NXOpen.Utilities.NXObjectManager.Get(this)).JournalIdentifier; } }

        [NotNull]
        public Part OwningPart
        {
            get
            {
                Tag owningPartTag;
                Globals.UFSession.Obj.AskOwningPart(_tag, out owningPartTag);
                return new Part(owningPartTag);
            }
        }

        public override int GetHashCode() { return (int) _tag; }

        [NotNull, Browsable(true)]
        public string Name
        {
            get
            {
                try
                {
                    string name;
                    UFSession.GetUFSession().Obj.AskName(_tag, out name);
                    return name;
                }
                catch
                {

                    return "";
                }
            }
            set { UFSession.GetUFSession().Obj.SetName(_tag, value); }
        }

        [NotNull]
        public TaggedObject TaggedObject { get { return NXOpen.Utilities.NXObjectManager.Get(Tag); } }

        [Pure, ContractAnnotation("null=>halt")]
        public bool HasStringAttribute([NotNull] string title)
        {
            string value;
            return GetStringAttribute(title, out value);
        }

        [NotNull, Pure, ContractAnnotation("null=>halt")]
        public string GetStringAttribute(string title)
        {
            string value;
            if (!GetStringAttribute(title, out value))
                throw new ArgumentException("Object does not have attribute with title \"" + title + "\".", "title");
            return value;
        }

        [Pure, ContractAnnotation("title:null=>halt")]
        protected bool GetStringAttribute(string title, out string value)
        {
            if (title == null) throw new ArgumentNullException("title");
            var iter = new UFAttr.Iterator();
            UFSession.GetUFSession().Attr.InitUserAttributeIterator(ref iter);
            string attributeValue;
            bool hasAttribute;
            UFSession.GetUFSession().Attr.GetStringUserAttribute(Tag, title, UFConstants.UF_ATTR_NOT_ARRAY, out attributeValue, out hasAttribute);
            value = attributeValue;
            return hasAttribute;
        }

        public override string ToString() { return _tag + ""; }

        public static NXObject CreateObject(NXOpen.Tag tag)
        {
            switch (tag.Type())
            {
                case UFConstants.UF_occ_instance_type:
                    // todo: need to figure out how to handle this.
                    return new NXObject(tag);
                case UFConstants.UF_solid_type:
                    switch (tag.Subtype())
                    {
                        case UFConstants.UF_solid_body_subtype:
                            return new Body(tag);
                        case UFConstants.UF_edge_3_subtype:
                            return new Edge(tag);
                        case UFConstants.FACE:
                            return new Face(tag);
                        default:
                            return new NXObject(tag);
                    }
                case UFConstants.UF_conic_type:
                    switch (tag.Subtype())
                    {
                        case UFConstants.UF_conic_ellipse_subtype:
                            return new Ellipse(tag);
                        case UFConstants.UF_conic_parabola_subtype:
                            return new Parabola(tag);
                        default:
                            return new NXObject(tag);
                    }

                case UFConstants.UF_reference_set_type:
                    return new ReferenceSet(tag);
                case UFConstants.UF_line_type:
                    return new Line(tag);
                case UFConstants.UF_circle_type:
                    return new Arc(tag);
                case UFConstants.UF_component_type:
                    return new Component(tag);
                case UFConstants.UF_datum_axis_type:
                    return new DatumAxis(tag);
                case UFConstants.UF_datum_plane_type:
                    return new DatumPlane(tag);
                case UFConstants.UF_coordinate_system_type: // 45
                    return new DatumCsys(tag);
                case UFConstants.UF_plane_type: // 46
                    return new Plane(tag);
                case UFConstants.UF_point_type:
                    return new Point(tag);
                case UFConstants.UF_drawing_type:
                    return new DrawingSheet(tag);
                case UFConstants.UF_drafting_entity_type:
                    switch (tag.Subtype())
                    {
                        case UFConstants.UF_draft_note_subtype:
                            return new Note(tag);
                        default:
                            return new NXObject(tag);
                    }
                case UFConstants.UF_feature_type:
                    return Feature.CreateFeature(tag);
                default:
                    return new NXObject(tag);
            }
        }
    }
}