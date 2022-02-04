using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;

namespace MadoriVR.Scripts.CreateHouseModel.ImageLoading.ImageLoading.Test
{
    public sealed class LoadedImageModelTest
    {
        [Test]
        public void SetPass_InputInvalidPath_ThrowException()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var validator = new ImagePathValidator(fileSystem);
            var model = new LoadedImageModel(validator);
            var notExistedPath = @"c:notExist.txt";
            
            Assert.That(() => model.SetPath(notExistedPath), Throws.TypeOf<ArgumentException>());
        }
    }
}