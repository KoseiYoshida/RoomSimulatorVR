using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MadoriVR.Scripts.ImageLoading
{
    public sealed class LoadedImageModel
    {
        private readonly ImagePathValidator pathValidator;
        private string path;
        public void SetPath(string candidate)
        {
            // FIX: validation, validatorをクラス化したらpreesenterでも使い回せるかも。
            var result = pathValidator.Validate(candidate);
            if (!result.isValid)
            {
                throw new ArgumentException($"Path:{candidate} is not valid. Reason:{result.notValidReason}");
            }
            
            this.path = candidate;
        }
        
        private readonly AsyncLazy<Texture2D> textureLoadLazy;
        public UniTask<Texture2D> GetTextureAsync() => textureLoadLazy.Task;
        
        public LoadedImageModel(ImagePathValidator pathValidator)
        {
            this.pathValidator = pathValidator;
            
            textureLoadLazy = UniTask.Lazy(async () =>
            {
                byte[] imageBytes;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    imageBytes = new byte[fs.Length];
                    await fs.ReadAsync(imageBytes, 0, (int) fs.Length);
                }

                var texture = new Texture2D(1, 1);
                texture.LoadImage(imageBytes);

                return texture;
            });
        }
    }
}