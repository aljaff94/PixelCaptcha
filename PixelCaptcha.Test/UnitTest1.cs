using System;
using Xunit;

namespace PixelCaptcha.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var val = Captcha.GenerateRandomString(5);
            PixelCaptcha.Captcha.GenerateImage(val);
        }
    }
}
