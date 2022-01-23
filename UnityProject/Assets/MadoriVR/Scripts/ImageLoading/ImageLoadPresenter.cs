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
                    // TODO: サニタイズ
                    var result = pathValidator.Validate(path);
                    if (!result.isValid)
                    {
                        Debug.Log($"not valid: {result.notValidReason}");
                        return;
                    }
                    
                    model.SetPath(path);
                    
                    var texture = await model.GetTextureAsync();
                    view.ShowImage(texture);
                }).AddTo(compositeDisposable);
        }
    }
}