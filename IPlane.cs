using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTSOpen.Interfaces
{
    /// <summary>Represents an axis with a coordinate serving as an origin and a vector coming off that origin to define the axis.</summary>
    public interface IPlane
    {
        /// <summary>The origin of the <see cref="IPlane.Vector"/>.</summary>
        ICoord Origin { get; }

        /// <summary>The normal of the <see cref="IPlane"/>.</summary>
        IVector Vector { get; }
    }
}
