using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTSOpen.Interfaces
{
    /// <summary>Represents an axis with a coordinate serving as an origin and a vector coming off that origin to define the axis.</summary>
    public interface IAxis
    {
        /// <summary>The origin of the Datum Axis.</summary>
        ICoord Origin { get; }

        /// <summary>The Normal of the Vector</summary>
        IVector Vector { get; }
    }
}
