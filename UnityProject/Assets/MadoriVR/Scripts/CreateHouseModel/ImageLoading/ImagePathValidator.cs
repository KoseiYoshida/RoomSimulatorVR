using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace MadoriVR.Scripts.CreateHouseModel.ImageLoading
{
    public sealed class ImagePathValidator
    {
        public static readonly IEnumerable<string> AllowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
        };

        private readonly IFileSystem fileSystem;
        public ImagePathValidator(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public (bool isValid, string notValidReason) Validate(string path)
        {
            // ファイルが存在するかどうか
            if (!fileSystem.File.Exists(path))
            {
                return (false, "File doesn't exist");
            }
            
            // 画像ファイルかどうか
            var extension = fileSystem.Path.GetExtension(path);
            if (!AllowedExtensions.Contains(extension.ToLowerInvariant()))
            {
                return (false, "Filetype is not image.");
            }

            return (true, "");
        }
    }
}