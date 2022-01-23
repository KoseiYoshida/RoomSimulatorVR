using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace MadoriVR.Scripts.ImageLoading
{
    public sealed class ImageLoadPresenter : IStartable
    {
        private readonly LoadedImageModel model;
        private readonly ImageLoadView view;
        private readonly ImagePathValidator pathValidator;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public ImageLoadPresenter(LoadedImageModel model, ImageLoadView view, ImagePathValidator pathValidator)
        {
            this.model = model;
            this.view = view;
            this.pathValidator = pathValidator;
        }

        public void Start()
        {
            view.OnPathEntered
                .Subscribe(async path =>
                {
                    var result = pathValidator.Validate(path);
                    
                    if (!result.isValid)
                    {
                        view.ShowLoadResult(result.notValidReason);
                        Debug.LogWarning($"not valid: {result.notValidReason}");
                        return;
                    }
                    
                    model.SetPath(path);
                    
                    view.ShowLoadResult("Loading.");
                    var texture = await model.GetTextureAsync();
                    view.ShowImage(texture);
                    view.ShowLoadResult("Loaded.");
                }).AddTo(compositeDisposable);
        }
    }
}