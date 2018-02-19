using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public class Edge : DisplayableObject
    {
        public Edge(Tag tag) : base(tag) { }

        [NotNull]
        public Body Body
        {
            get
            {
                Tag bodyTag;
                UFSession.GetUFSession().Modl.AskEdgeBody(Tag, out bodyTag);
                return new Body(bodyTag);
            }
        }

        [NotNull, ItemNotNull]
        public Face[] Faces
        {
            get
            {
                Tag[] faceTags;
                UFSession.GetUFSession().Modl.AskEdgeFaces(Tag, out faceTags);
                var returnFaces = new Face[faceTags.Length];
                for (var i = 0; i < faceTags.Length; i++)
                    returnFaces[i] = new Face(faceTags[i]);
                return returnFaces;
            }
        }
    }
}
