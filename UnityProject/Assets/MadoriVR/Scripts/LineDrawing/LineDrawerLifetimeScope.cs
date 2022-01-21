using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.LineDrawing
{
    [RequireComponent(typeof(LineDrawEventProvider))]
    public sealed class LineDrawerLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<DrawLineModel>(Lifetime.Singleton);
            builder.RegisterInstance(GetComponent<LineDrawEventProvider>());
            builder.RegisterEntryPoint<LineDrawInput>();
            builder.RegisterInstance(GetComponent<LineDrawView>());
            builder.RegisterEntryPoint<LineDrawPresenter>();

        }
    }
}