using System.Collections.Generic;

namespace CTSOpen.Interfaces
{
    /// <inheritdoc />
    /// <summary>Defines a 3D coordinate in space. And it will always have 3 values as it's enumerated. (X[0], Y[1], Z[2]).</summary>
    public interface ICoord //: IEnumerable<double>
    {
        //<summary>The x-value of the coordinate.</summary>
        double X { get; }

        //<summary>The y-value of the coordinate.</summary>
        double Y { get; }

        //<summary>The z-value of the coordinate.</summary>
        double Z { get; }
    }
}