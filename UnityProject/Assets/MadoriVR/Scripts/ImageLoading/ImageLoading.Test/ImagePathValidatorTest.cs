using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MadoriVR.Scripts.ImageLoading.ImageLoading.Test
{
    public sealed class ImagePathValidatorTest
    {
        [Test]
        public void Validate_Success()
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
        public void Validate_Failed_FileNotExist()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var validator = new ImagePathValidator(fileSystem);
            var notExistPath = @"c:/notExist.txt";

            var result = validator.Validate(notExistPath);
            
            Assert.That(result.isValid, Is.False);
        }
        
        [Test]
        public void Validate_Failed_FileTypeIsNotImage()
        {
            var path = @"c:\sampleFile" + ".txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {path, new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 })},
            });
            var validator = new ImagePathValidator(fileSystem);

            var result = validator.Validate(path);
            
            Assert.That(result.isValid, Is.False);
        }
    }
}