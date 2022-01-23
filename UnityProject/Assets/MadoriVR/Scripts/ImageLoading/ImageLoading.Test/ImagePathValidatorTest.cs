using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NUnit.Framework;

namespace MadoriVR.Scripts.ImageLoading.ImageLoading.Test
{
    public sealed class ImagePathValidatorTest
    {
        [Test]
        public void Validate_InputCorrectPath_Success()
        {
            var ex = ImagePathValidator.AllowedExtensions.First();
            var path = @"c:\sampleFile" + ex;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {path, new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 })},
            });
            var validator = new ImagePathValidator(fileSystem);

            var result = validator.Validate(path);
            
            Assert.That(result.isValid, Is.True);
        }

        [Test]
        public void Validate_InputNotExistFilePath_Failed()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var validator = new ImagePathValidator(fileSystem);
            var notExistPath = @"c:/notExist.txt";

            var result = validator.Validate(notExistPath);
            
            Assert.That(result.isValid, Is.False);
        }
        
        [Test]
        public void Validate_InputNotImageFilePath_Failed()
        {
            var path = @"c:\sampleFile" + ".txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {path, new MockFileData("hoge")},
            });
            var validator = new ImagePathValidator(fileSystem);

            var result = validator.Validate(path);
            
            Assert.That(result.isValid, Is.False);
        }
    }
}