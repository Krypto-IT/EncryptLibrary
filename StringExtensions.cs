using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptLibrary
{
    public static class StringExtensions
    {
        public static string Left(this string value, int count)
        {
            var l = value.Length;
            if(l >= count)
            {
                return value.Substring(0, count);
            }
            return value;
        }
    }
}
