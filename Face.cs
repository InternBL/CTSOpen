using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public class Face : DisplayableObject
    {
        public Face(Tag tag) : base(tag)
        {
        }

        [NotNull,ItemNotNull]
        public Edge[] Edges
        {
            get
            {
                Tag[] edgeTags;
                UFSession.GetUFSession().Modl.AskFaceEdges(Tag, out edgeTags);
                var returnEdges = new Edge[edgeTags.Length];
                for(var i = 0; i < edgeTags.Length; i++)
                    returnEdges[i] = new Edge(edgeTags[i]);
                return returnEdges;
            }
        }

        [NotNull]
        public Body Body
        {
            get
            {
                Tag bodyTag;
                UFSession.GetUFSession().Modl.AskFaceBody(Tag, out bodyTag);
                return new Body(bodyTag);
            }
        }
    }
}