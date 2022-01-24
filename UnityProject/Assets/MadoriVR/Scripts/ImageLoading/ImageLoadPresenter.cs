using System;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace MadoriVR.Scripts.ImageLoading
{
    public sealed class ImageLoadPresenter : IStartable, IDisposable
    {
        private readonly LoadedImageModel model;
        private readonly IImageSelector imageSelector;
        private readonly IImageShower imageShower;
        private readonly ImagePathValidator pathValidator;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public ImageLoadPresenter(LoadedImageModel model, IImageSelector imageSelector, IImageShower imageShower, ImagePathValidator pathValidator)
        {
            this.model = model;
            this.imageSelector = imageSelector;
            this.imageShower = imageShower;
            this.pathValidator = pathValidator;
        }

        public void Start()
        {
            imageSelector.OnPathEntered()
                .Subscribe(async path =>
                {
                    var result = pathValidator.Validate(path);
                    
                    if (!result.isValid)
                    {
                        imageShower.ShowLoadResult(result.notValidReason);
                        Debug.LogWarning($"not valid: {result.notValidReason}");
                        return;
                    }
                    
                    model.SetPath(path);
                    
                    imageShower.ShowLoadResult("Loading.");
                    var texture = await model.GetTextureAsync();
                    imageShower.ShowImage(texture);
                    imageShower.ShowLoadResult("Loaded.");
                }).AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}