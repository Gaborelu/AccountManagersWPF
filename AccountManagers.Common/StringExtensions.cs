
using System;

namespace AccountManagers.Common
{
    public static class StringExtensions
    {

        public static string Mask(this string @this, int startIndex, int endIndex)
        {
            var characters = @this.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                if (i >= startIndex && i < endIndex)
                {
                    characters[i] = '*';
                }
            }

            return new string(characters);
            if (string.IsNullOrEmpty(@this))
            {
                return string.Empty;
            }

            if(startIndex>@this.Length || endIndex > @this.Length)
            {
                throw new ArgumentException();
            }

            
            
        }
    }
}
