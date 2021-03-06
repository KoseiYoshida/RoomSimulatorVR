using System.IO.Abstractions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.ImageLoading
{
    [RequireComponent(typeof(ImageLoadView))]
    public sealed class ImageLoadLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<ImagePathValidator>(Lifetime.Singleton).WithParameter("fileSystem", new FileSystem());
            builder.Register<LoadedImageModel>(Lifetime.Singleton);
            builder.RegisterInstance<ImageLoadView>(GetComponent<ImageLoadView>())
                .As<IImageSelector>();
            builder.RegisterEntryPoint<ImageLoadPresenter>();
        }
    }
}