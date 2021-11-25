
using System;

namespace AccountManagers.Common
{
    public static class StringExtensions
    {

        public static string Mask(this string @this, int startIndex, int endIndex)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return string.Empty;
            }

            if(startIndex>@this.Length || endIndex > @this.Length)
            {
                throw new ArgumentException();
            }

            return string.Empty;
        }
    }
}
