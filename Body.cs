using JetBrains.Annotations;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public class Body :DisplayableObject
    {
        public Body(Tag tag) : base(tag)
        {
        }


        [NotNull, ItemNotNull]
        public Edge[] Edges
        {
            get
            {
                Tag[] edgeTags;
                UFSession.GetUFSession().Modl.AskBodyEdges(Tag, out edgeTags);
                var returnEdges = new Edge[edgeTags.Length];
                for (var i = 0; i < edgeTags.Length; i++)
                    returnEdges[i] = new Edge(edgeTags[i]);
                return returnEdges;
            }
        }

        [NotNull, ItemNotNull]
        public Face[] Faces
        {
            get
            {
                Tag[] faceTags;
                UFSession.GetUFSession().Modl.AskBodyFaces(Tag, out faceTags);
                var returnFaces = new Face[faceTags.Length];
                for (var i = 0; i < faceTags.Length; i++)
                    returnFaces[i] = new Face(faceTags[i]);
                return returnFaces;
            }
        }



    }
}
