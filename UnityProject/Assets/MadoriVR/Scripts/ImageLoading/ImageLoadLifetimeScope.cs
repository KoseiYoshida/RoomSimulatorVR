using System.IO.Abstractions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.ImageLoading
{
    [RequireComponent(typeof(ImageLoadView))]
    public sealed class ImageLoadLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance<ImagePathValidator>(new ImagePathValidator(new FileSystem()));
            builder.Register<LoadedImageModel>(Lifetime.Singleton);
            builder.RegisterInstance<ImageLoadView>(GetComponent<ImageLoadView>());
            builder.RegisterEntryPoint<ImageLoadPresenter>();
        }
    }
}