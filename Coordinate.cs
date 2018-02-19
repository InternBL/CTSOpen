using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTSOpen.Interfaces;
using Snap;

namespace CTSOpen
{
    public struct Coordinate : ICoord
    {


        public Coordinate(IEnumerable<double> coordinates)
        {
            var array = coordinates.ToArray();
            if (array.Length != 3) throw new ArgumentOutOfRangeException("coordinates");
            _x = array[0];
            _y = array[1];
            _z = array[2];
        }

        public Coordinate(ICoord iCoord) : this(iCoord.X, iCoord.Y, iCoord.Z) { }









        private readonly double _x, _y, _z;

        public double X { get { return _x; } }

        public double Y { get { return _y; } }

        public double Z { get { return _z; } }

        public double[] Coords { get { return new[] {_x, _y, _z}; } }

        public Coordinate(double x, double y, double z)
            : this(new[] {x, y, z})
        {
        }


        #region Casting Operators


        public static implicit operator Coordinate(double[] coordinates) { return new Coordinate(coordinates); }

        public static implicit operator double[](Coordinate coordinate) { return coordinate.Coords; }

        public static implicit operator Coordinate(Snap.Position position) { return new Coordinate(position.X, position.Y, position.Z); }

        public static implicit operator Snap.Position(Coordinate coordinate) { return new Position(coordinate.Coords); }

        #endregion


    }
}