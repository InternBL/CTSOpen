using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace CTSOpen.NX
{
    /// <summary>
    /// This a basic plane in NX. The triangle version.
    /// I beleive the difference is that this one is not a Feature.
    /// </summary>
    public  class Plane : DisplayableObject
    {
        public Plane(Tag tag) : base(tag)
        {
        }
    }
}
