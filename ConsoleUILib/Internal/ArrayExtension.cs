using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Internal
{
    internal static class ArrayExtension
    {
        public static T[,] Resize<T>(this T[,] arr, int w, int h)
        {
            var newArray = new T[w, h];
            int minRows = Math.Min(w, arr.GetLength(0));
            int minCols = Math.Min(h, arr.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = arr[i, j];
            return newArray;
        }
    }
}
