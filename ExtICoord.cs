using System;
using CTSOpen.Interfaces;
using NXOpen.UF;

namespace CTSOpen.Extensions
{
    public static class ExtICoord
    {
        public const double Tolerance = .0001;

        public static Coordinate ToCoord(this ICoord iCoord)
        {
            return new Coordinate(iCoord);
        }


        public static bool __Parallel(this ICoord coord1, ICoord coord2 , double tolerance = Tolerance)
        {
            int result;
            UFSession.GetUFSession().Vec3.IsParallel(coord1.ToCoord(),coord2.ToCoord(),tolerance,out result);
            switch (result)
            {
                case UFConstants.TRUE:
                    return true;
                case UFConstants.FALSE :
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }




    }
}
