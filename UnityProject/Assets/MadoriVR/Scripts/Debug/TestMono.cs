using System.Threading;
using Cysharp.Threading.Tasks;
using MadoriVR.Scripts.Debug;
using MadoriVR.Scripts.ImageLoading;
using MadoriVR.Scripts.LineDrawing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR
{
    public sealed class TestMono : LifetimeScope
    {
        [SerializeField] private LifetimeScope lineDrawLifetimeScope = default;
        private DrawnLineModel _drawnLineModel;

        [SerializeField] private LifetimeScope imageLoadLifetimeScope = default;
    
        private void Start()
        {
            // 最初は画像の読み込み
            // 読み込みが終わったら、読み込み用UIを見えなくする。
            // 画像が表示されるので、ラインを引くモードにする
            // ラインが引かれたら、GenerateButtonを押してモデルをだす。
        }

        protected override void Awake()
        {
            base.Awake();

            // TODO: 親子関係の作り方。ImageLoaderのPrefab化 + EnqueueParentをつかって、ImageLoader側のIContainerBuilderにIImageShowerを登録してやるのが綺麗なやり方かも。検討する。
            imageLoadLifetimeScope.parentReference.Object = this;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(GetComponent<ImageShower>()).As<IImageShower>();
            builder.RegisterEntryPoint<SequenceController>();
        }
    }

    public sealed class SequenceController : IAsyncStartable
    {
        private readonly ImageShower imageShower;
        
        [Inject]
        public SequenceController(ImageShower imageShower)
        {
            this.imageShower = imageShower;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await imageShower.OnImageLoaded;
            
            // LineDrawの有効化
            // LineDrawの結果を受け取り、Generate。（Generate）
            
            // 次のシーン読みこみ？
        }
    }
}