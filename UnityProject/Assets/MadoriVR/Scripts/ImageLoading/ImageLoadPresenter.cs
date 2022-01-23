using UniRx;
using VContainer.Unity;

namespace MadoriVR.Scripts.ImageLoading
{
    public sealed class ImageLoadPresenter : IStartable
    {
        private readonly LoadedImageModel model;
        private readonly ImageLoadView view;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public ImageLoadPresenter(LoadedImageModel model, ImageLoadView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Start()
        {
            view.OnPathEntered
                .Subscribe(async path =>
                {
                    // TODO: サニタイズ
                    model.SetPath(path);
                    var texture = await model.GetTextureAsync();
                    view.ShowImage(texture);
                }).AddTo(compositeDisposable);
        }
    }
}