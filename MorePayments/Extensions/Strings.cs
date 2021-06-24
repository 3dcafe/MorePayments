using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Extensions
{
    /// <summary>
    /// Summary description for Strings
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Try parse bool
        /// </summary>
        /// <param name="val"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        public static bool? TryParseBool(this string val, bool isNullable)
        {
            bool _val;
            return Boolean.TryParse(val, out _val) || !isNullable ? _val : (bool?)null;
        }

    }
}
