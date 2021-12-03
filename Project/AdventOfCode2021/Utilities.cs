using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBExtensions
{
    public static class SbExtensions
    {
        public static int ToInt(this BitArray bits)
        {
            var result = new int[1];
            bits.CopyTo(result, 0);
            return result[0];
        }

        public static string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
