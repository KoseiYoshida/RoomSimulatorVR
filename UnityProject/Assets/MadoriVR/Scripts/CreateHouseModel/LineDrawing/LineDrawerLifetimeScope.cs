using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.LineDrawing
{
    /// <summary>
    /// Compose line draw classes dependency.
    /// </summary>
    [RequireComponent(typeof(LineDrawEventProvider))]
    public sealed class LineDrawerLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<DrawnLineModel>(Lifetime.Singleton);
            builder.RegisterInstance(GetComponent<LineDrawEventProvider>());
            builder.RegisterEntryPoint<LineDrawInput>();
            builder.RegisterInstance(GetComponent<LineDrawView>());
            builder.RegisterEntryPoint<LineDrawPresenter>();
            builder.RegisterEntryPoint<LineSupplier>();
        }
    }
}