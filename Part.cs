using System;
using System.Collections.Generic;
using System.Linq;
using CTSOpen.Extensions;
using CTSOpen.NX.Features;
using JetBrains.Annotations;
using NXOpen;
using NXOpen.Drawings;
using NXOpen.UF;
using Snap;

namespace CTSOpen.NX
{
    //[DebuggerStepThrough]
    public class Part : NXObject
    {
        public Snap.NX.Part SnapPart { get { return Snap.NX.Part.Wrap(Tag); } }

        public NXOpen.Part NXOpenPart { get { return SnapPart.NXOpenPart; } }

        public Part(Tag tag) : base(tag) { }

        ///<summary>Returns the top level <see cref="Component"/> of this <see cref="Part"/>.</summary>
        ///<remarks>Returns null if this <see cref="Part"/> doesn't have any <see cref="Component"/>'s under it.</remarks>
        [CanBeNull]
        public Component RootComponent
        {
            [Pure]
            get
            {
                //return SnapPart.RootComponent == null ? null : new Component(SnapPart.RootComponent.NXOpenTag);
                var rootPartOccTag = Globals.UFSession.Assem.AskRootPartOcc(Tag);
                return rootPartOccTag == NXOpen.Tag.Null
                    ? null
                    : new Component(rootPartOccTag);
            }
        }

        [NotNull]
        public string FullPath
        {
            [Pure]
            get
            {
                string filePath;
                Globals.UFSession.Part.AskPartName(Tag, out filePath);
                return filePath;
            }
        }

        [NotNull]
        public string Leaf { [NotNull,Pure] get { return System.IO.Path.GetFileNameWithoutExtension(FullPath); } }

        //[NotNull, ItemNotNull]
        //public NXOpen.Features.Feature[] Features
        //{

        //    // todo: attributes seem weird here
        //    // todo: also UFSession this.
        //    [  Pure]
        //    get
        //    {
        //        //var list = new List<NXOpen.Features.Feature>();
        //        //NXOpen.Tag currentFeatureTag;
        //        //UFSession.GetUFSession().Modl.AskCurrentFeature(Tag, out currentFeatureTag);
        //        //if(currentFeatureTag == Tag.Null)return new Feature[0];

                




        //        return


                    
        //            Snap.NX.Part.Wrap(Tag).NXOpenPart.Features.ToArray();
        //    }
        //}

        [NotNull, ItemNotNull]
        public Feature[] Features
        {
                get
                {
                    var list = new List<Feature>();
                    var tag = Tag.Null;
                    do
                    {
                        UFSession.GetUFSession().Obj.CycleTypedObjsInPart(Tag, UFConstants.UF_feature_type, ref tag);
                        if (tag != Tag.Null)
                            list.Add(Feature.CreateFeature(tag));

                    } while (tag != Tag.Null);
                    return list.ToArray();
                }
        }

        [NotNull, ItemNotNull]
        public Snap.NX.Expression[] Expressions
        {
            get
            {
                // todo: UFSesion
                return Snap.NX.Part.Wrap(Tag).Expressions;

            }
        }
        
        [Obsolete("Make this an extension.")]
        public bool HasDynamicBlock { get { return Features.SingleOrDefault(feature => feature.Name == "DYNAMIC BLOCK") != null; } }

        [NotNull, ItemNotNull]

        public DrawingSheet[] DrawingSheets
        {
            get
            {
                var list = new List<DrawingSheet>();
                var tag = Tag.Null;
                do
                {
                    UFSession.GetUFSession().Obj.CycleTypedObjsInPart(Tag, UFConstants.UF_drawing_type, ref tag);
                    if (tag != Tag.Null)
                        list.Add(new DrawingSheet(tag));
                } while (tag != Tag.Null);

                return list.ToArray();
            }
        }

        /// <summary>
        /// Gets the reference set in this part. 
        /// Returns null if a reference set with that name isn't found.
        /// </summary>
        /// <param name="referenceSetName"></param>
        [CanBeNull,ContractAnnotation("null=>halt")]
        public ReferenceSet ReferenceSet([NotNull] string referenceSetName)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (referenceSetName == "") throw new ArgumentException("Reference Set name cannot be empty.", "referenceSetName");
            if (referenceSetName == null) throw new ArgumentNullException("referenceSetName");
            return ReferenceSets.SingleOrDefault(set => set.Name == referenceSetName);
        }

        /// <summary>Returns all the names of the Reference Sets contained within in this Part.</summary>
        /// <remarks>The "Empty" reference set name is returned, the "Entire Part" reference set name is not returned.</remarks>
        [NotNull, ItemNotNull]
        public string[] ReferenceSetName { get { return ReferenceSets.Select(set => set.Name).ToArray(); } }

        /// <summary>Returns all the Reference Sets contained within in this Part.</summary>
        /// <remarks>The "Empty" reference set is returned, the "Entire Part" reference set is not returned.</remarks>
        [NotNull, ItemNotNull]
        public ReferenceSet[] ReferenceSets
        {
            get
            {
                var list = new List<ReferenceSet>();
                var tag = Tag.Null;
                do
                {
                    UFSession.GetUFSession().Obj.CycleTypedObjsInPart(Tag, UFConstants.UF_reference_set_type, ref tag);
                    if(tag != Tag.Null)
                        list.Add(new ReferenceSet(tag));
                } while (tag != Tag.Null);
                return list.ToArray();
            }
        }
        
        public override string ToString() { return FullPath; }
    }
}