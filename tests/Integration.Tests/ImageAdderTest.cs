using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using NUnit.Framework;
using Services;

namespace Integration.Tests
{
    public class ImageAdderTest
    {
        public ImageAdderTest()
        {
            _imageAdder = new ImageAdder();
        }

        //??? need to figure out how to mock or instantiate an IBrowserFile for testing
        //[Test]
        //public async Task VerifyValidationWorks()
        //{
        //    //Arrange
        //    IBrowserFile browserFile;
        //    long fileSizeLimit = 2097152;   // 2 MB

        //    //Act
        //    byte[] imageByteArray = await _imageAdder.Validate(browserFile, fileSizeLimit);

        //    //Assert
        //    Assert.That(imageByteArray.Length == 0);
        //}

        #region private

        readonly IImageAdder _imageAdder;

        #endregion
    }
}
