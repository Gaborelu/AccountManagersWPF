using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AccountManagers.Common.Tests
{    
    public class StringExtensionsTests
    {
        

        [Fact]
        public void MaskShouldReturnStringEmptyWhenInputIsEmpty()
        {
            //Given
            var input = string.Empty;
            //When
            var result = input.Mask(1, 2);

            //Then
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MaskShouldReturnMaskedStringWhenIndexesAreInRange()
        {
            //Given
            var input = "1880720226720";

            //When
            var result = input.Mask(0, 2);
            //Then

            Assert.Equal("**80720226720", result);
        }

        [Fact]
        public void MaskShouldThrowArgumentExceptionWhenIndexOutOfRange()
        {
            var input = "188072022670";

            Assert.Throws<ArgumentException>(() => input.Mask(2, 100));
        }


    }
}
