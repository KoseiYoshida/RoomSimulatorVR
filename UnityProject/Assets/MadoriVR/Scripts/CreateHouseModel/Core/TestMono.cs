using MadoriVR.Scripts.CreateHouseModel.ImageLoading;
using MadoriVR.Scripts.CreateHouseModel.LineDrawing;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.Core
{
    public sealed class TestMono : LifetimeScope
    {
        [SerializeField] private LifetimeScope lineDrawLifetimeScope = default;
        private DrawnLineModel _drawnLineModel;

        [SerializeField] private LifetimeScope imageLoadLifetimeScope = default;

        private readonly LineDrawController lineDrawController = new ();
        [FormerlySerializedAs("button")] [SerializeField] private Button generateHouseModelButton;
        
        
        protected override void Awake()
        {
            base.Awake();

            // TODO: 親子関係の作り方。ImageLoaderのPrefab化 + EnqueueParentをつかって、ImageLoader側のIContainerBuilderにIImageShowerを登録してやるのが綺麗なやり方かも。検討する。
            imageLoadLifetimeScope.parentReference.Object = this;
            lineDrawLifetimeScope.parentReference.Object = this;
        }
        
        // TODO: Compose以外の処理は別クラスに分ける。
        private void Start()
        {
            generateHouseModelButton.OnClickAsObservable()
                .Subscribe(_ => lineDrawController.CanDraw().Value = false)
                .AddTo(this);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(GetComponent<ImageShower>()).As<IImageShower>();
            //builder.Register<LineDrawController>(Lifetime.Singleton).As<ILineDrawClient>().AsSelf();
            builder.RegisterInstance(lineDrawController).As<ILineDrawClient>().AsSelf();
            builder.RegisterEntryPoint<SequenceController>();
        }
    }
}