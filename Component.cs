using System;
using System.Collections.Generic;
using System.Linq;
using CTSOpen.Extensions;
using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;
using Snap;

namespace CTSOpen.NX
{
    public class Component : DisplayableObject
    {
        public static bool IsComponent(NXOpen.Tag tag)
        {
            return UFSession.GetUFSession().Assem.IsPartOccurrence(tag);
        }

        public Snap.NX.Component SnapComponent { get { return Snap.NX.Component.Wrap(Tag); } }

        public NXOpen.Assemblies.Component NXOpenComponent { get { return SnapComponent.NXOpenComponent; } }

        public Component(Tag tag) : base(tag) { if (!IsComponent(tag))throw new ArgumentOutOfRangeException("tag", "Tag was not a component."); }
        
        public bool Suppressed
        {
            get
            {
                bool suppressed;
                bool anyParentSuppressed;
                UFSession.GetUFSession().Assem.AskPartOccSuppressState(Tag, out suppressed, out anyParentSuppressed);
                return anyParentSuppressed || suppressed;
            }
            set
            {
                switch (value)
                {
                    case true:
                        NXOpenComponent.Suppress();
                        break;
                    case false:
                        NXOpenComponent.Unsuppress();
                        break;
                        default:
                            throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        public bool IsNgc
        {
            //UF_ASSEM_is_component_ngc (view source)
            //Defined in: uf_assem.h
            //    Overview
            //UF_ASSEM_is_component_ngc 
            //    Queries whether the input component is a non-geometric component (NGC) 
            //or not. 
            //    Return
            //Returns TRUE if component_tag is a NGC, FALSE otherwise. 
            //    Environment
            //    This function is supported for both Internal and 
            //    External execution. 
            //    History
            //    Initially released in NX6.0. 
            //Required License(s)
            //gateway
            //    logical UF_ASSEM_is_component_ngc
            //(
            //    tag_t component_tag 
            //)
            //tag_t component_tag Input the occurence or instance tag 
            get { return UFSession.GetUFSession().Assem.IsComponentNgc(Tag); }
        }

     
        [NotNull]
        public string DisplayName
        {
            get
            {
                string partName;
                string referenceSetName;
                string instanceName;
                var origin = new double[3];
                var matrix = new double[9];
                var transform = new double[4, 4];
                UFSession.GetUFSession().Assem.AskComponentData(Tag, out partName, out referenceSetName, out instanceName, origin, matrix, transform);
                var str = System.IO.Path.GetFileNameWithoutExtension(partName);
                return str ?? partName;
            }
        }

        public Coordinate Origin
        {
            get
            {
                string partName;
                string referenceSetName;
                string instanceName;
                var origin = new double[3];
                var matrix = new double[9];
                var transform = new double[4, 4];
                UFSession.GetUFSession().Assem.AskComponentData(Tag, out partName, out referenceSetName, out instanceName, origin, matrix, transform);
                return new Coordinate(origin[0],origin[1],origin[2]);
            }
        }

        public Snap.Orientation Orientation
        {
            get
            {
                string partName;
                string referenceSetName;
                string instanceName;
                var origin = new double[3];
                var matrix = new double[9];
                var transform = new double[4, 4];
                UFSession.GetUFSession().Assem.AskComponentData(Tag, out partName, out referenceSetName, out instanceName, origin, matrix, transform);
                return new Snap.Orientation(matrix);
            }
        }

        [NotNull, ItemNotNull]
        public IEnumerable<NXObject> Members
        {
            get
            {
                if (ReferenceSet == "Entire Part")
                    throw new InvalidOperationException("You can't get the objects from a component that is set to Entire Part yet. Weird stuff happens.");
                if (Prototype == null) throw new InvalidOperationException("Not loaded");
                foreach (var member in Prototype.ReferenceSet(ReferenceSet).Members)
                {
                    if (member.Tag.Type() == UFConstants.UF_occ_instance_type)
                    {
                        yield return new Component(UFSession.GetUFSession().Assem.AskPartOccOfInst(Tag, member));
                        continue;
                    }

                    var occurenceTag = UFSession.GetUFSession().Assem.FindOccurrence(Tag, member);
                    if (occurenceTag == NXOpen.Tag.Null)
                    {
                        //InfoWindow.WriteLine();
                        InfoWindow.WriteLine("Null Tag: " + member.GetType() + " " + Snap.NX.NXObject.Wrap(member.Tag).NXOpenTaggedObject.GetType() + " " +
                                             member.Tag.Type() + " " + member.Tag.Subtype());
                        continue;
                    }
                    yield return occurenceTag.ToCtsOpen();
                }
            }
        }

        public string ReferenceSet
        {
            //UF_ASSEM_replace_refset (view source)
            //Defined in: uf_assem.h
            //    Overview
            //Replace the reference set used by one or more instances or part 
            //occurrences held in the target_tags array. If an instance is specified, 
            //all part occurrences of that instance will use the new reference set. 
            //    This routine only works for immediate children of the work part. 
            //    If new_refset_name is a NULL pointer or a zero-length string, the 
            //    entire part is used. If it is the string "Empty", then none of the part 
            //is displayed. 
            //    Environment
            //    Internal and External 
            //Required License(s)
            //gateway
            //int UF_ASSEM_replace_refset
            //(
            //    int count, 
            //    tag_t * target_tags, 
            //const char * new_refset_name 
            //)
            //int count Input Count of part occurrences 
            //    tag_t * target_tags Input count 
            //    Array of instance and/or part occurrence tags 
            //const char * new_refset_name Input Name of new reference set; must be no longer 
            //than UF_OBJ_NAME_NCHARS characters. 
            get
            {
                string partName;
                string referenceSetName;
                string instanceName;
                var origin = new double[3];
                var matrix = new double[9];
                var transform = new double[4, 4];
                UFSession.GetUFSession().Assem.AskComponentData(Tag, out partName, out referenceSetName, out instanceName, origin, matrix, transform);
                return referenceSetName;
            }
            set
            {
                // todo: there has to be a way using the UFsession
                NXOpenComponent.DirectOwner.ReplaceReferenceSet(NXOpenComponent,value);
            }
        }



        public bool IsLoaded
        {
            get
            {
                var status = UFSession.GetUFSession().Part.IsLoaded(DisplayName);
                switch (status)
                {
                    // Part is not loaded
                    case 0:
                        return false;
                    // Part is fully loaded in session.
                    case 1:
                    // Part is partially loaded in session.
                    case 2:
                        return true;
                    // Anything else
                    default:
                        throw NXException.CreateWithoutUndoMark(status);
                }
            }
        }

        /// <summary>Returns the children of this <see cref="Component"/> that are loaded and not suppressed.</summary>
        /// <remarks>If you want to include Suppressed or unloaded children please use <seealso cref="AllChildren"/>.</remarks>
        [NotNull, ItemNotNull]
        public Component[] Children
        {
            get
            {
                return AllChildren.Where(component => !component.Suppressed && component.IsLoaded).ToArray();
            }
        }

        /// <summary>Returns all of the children of this <see cref="Component"/> regardless of their load or suppression state.</summary>
        [NotNull, ItemNotNull]
        public Component[] AllChildren
        {
            get
            {
               
                var nxChildren = NXOpenComponent.GetChildren();
                if(nxChildren== null || nxChildren.Length == 0)return new Component[0];
                return nxChildren.Select(component => new Component(component.Tag)).ToArray();
            }
        }

        /// <summary>Returns an enumerable collection containing the component itself, children, grandchildren, and all other descendents.</summary>
        [NotNull, ItemNotNull]

        public IEnumerable<Component> Descendants
        {
             get { return FindDescendants(this); }
        }


        /// <summary>Returns an enumerable collection containing the component itself, children, grandchildren, and all other descendents.</summary>
        /// <exception cref="ArgumentNullException">When <paramref name="parentComponent"/> is null.</exception>
        [ContractAnnotation("null=>halt"), NotNull, ItemNotNull, Pure]
        private static IEnumerable<Component> FindDescendants([NotNull] Component parentComponent)
        {
            if (parentComponent == null) throw new ArgumentNullException("parentComponent");
            yield return parentComponent;
            foreach (var child in parentComponent.Children)
            foreach (var descendant in FindDescendants(child))
                yield return descendant;
        }


        /// <summary>
        /// Need to fill out. Look at AskPrototypeOfOcc
        /// </summary>
        [CanBeNull]
        public Part Prototype
        {
            get
            {
                //return IsLoaded ? null : new Part(SnapComponent.Prototype.NXOpenTag);
                var prototypeOfPartOcc = UFSession.GetUFSession().Assem.AskPrototypeOfOcc(Tag);
                return prototypeOfPartOcc == NXOpen.Tag.Null
                    ? null
                    : new Part(prototypeOfPartOcc);
            }
        }
        
        public override string ToString() { return DisplayName; }
    }
}