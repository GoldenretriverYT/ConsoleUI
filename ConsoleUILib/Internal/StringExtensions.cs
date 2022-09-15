using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Internal
{
    internal static class StringExtensions
    {
        public static string Repeat(this string str, int amount)
        {
            if (str == null) throw new Exception("String can not be null.");
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Amount must be above 0");

            string output = "";

            for(int i = 0; i< amount; i++)
            {
                output += str;
            }

            return output;
        }
    }
}
