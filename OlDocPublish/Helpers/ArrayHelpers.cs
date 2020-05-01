using System;

namespace Helpers.Array
{
    public static class Searchers
    {
        // public static T[] Search<T>(this T[,] array2D, Predicate<int> searchParam, int dimension = 0)
        // {
        //     T[] result = new T[array2D.GetLength(dimension)];
        //     int Tindex = 0;

        //     int otherDimension = dimension > 0?0:1;
        //     int otherLength = array2D.GetLength(otherDimension);

        //     for(int arrayIndex = 0; arrayIndex < otherLength; arrayIndex++)
        //     {
        //         if(searchParam(arrayIndex))
        //         {
        //             result[Tindex] = 
        //         };
        //     }


        //     return result;
        // }

        public static T[] GetRow<T>(this T[,] array2D, int rowIndex)
        {
            T[] result = new T[array2D.GetLength(0)];
            int fieldCount = result.Length;

            for(int i = 0; i < fieldCount; i++)
            {
                result[i] = array2D[i, rowIndex];
            }

            return result;
        }

        //public static T[,] SelectRows<T>(this T[,] recordSet, Func<T[], bool> match)
        //{
        //    int fieldCount = recordSet.GetLength(0);
        //    int rowCount = recordSet.GetLength(1);
        //    for(int row = 0; row < rowCount; row++)
        //    {

        //    }
            
        //}
    } 
}