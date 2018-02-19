using System;
using System.Collections.Generic;
using System.Linq;
using CTSOpen.Extensions;
using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public class ReferenceSet : NXObject
    {
        public ReferenceSet(Tag tag) : base(tag)
        {
            int type;
            int subtype;
            UFSession.GetUFSession().Obj.AskTypeAndSubtype(tag, out type, out subtype);
            if (type != UFConstants.UF_reference_set_type)
                throw new ArgumentException("Not a reference set type.", "tag");
        }


        [NotNull, ItemNotNull]
        public IEnumerable<NXObject> Members
        {
            get
            {
                int count;
                Tag[] members;
                //This method does not return the components that are apart of the reference set.
                // It actually returns the Part Occurence instances.
                UFSession.GetUFSession().Assem.AskRefSetMembers(Tag, out count, out members);
                //if (OwningPart.RootComponent == null) return members.ToCtsOpen().ToArray();
                //var children = new List<Tag>();
                //foreach (var component in OwningPart.RootComponent.Children)
                //{
                //    if (component.IsSuppressed) continue;
                //    if (!component.IsLoaded) continue;
                //    bool flag;
                //    UFSession.GetUFSession().Assem.IsRefSetMember(component.Tag, out flag);
                //    if (!flag) continue;
                //    Tag[] refsets;
                //    UFSession.GetUFSession().Assem.AskRefSets(component.Tag, out count, out refsets);
                //    if (refsets.Any(tag => tag == Tag))
                //        children.Add(component.Tag);
                //}

                //children.AddRange(members);
                //members = children.ToArray();
                return members.ToCtsOpen().ToArray();
            }
        }
    }
}