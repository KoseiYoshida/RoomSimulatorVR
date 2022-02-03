using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadoriVR.Scripts.Debug;
using MadoriVR.Scripts.ImageLoading;
using MadoriVR.Scripts.LineDrawing;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MadoriVR
{
    public sealed class LineDrawController : ILineDrawClient, IDisposable
    {
        private readonly UniTaskCompletionSource<IEnumerable<ImmutableLine>> onLineDrawFinishedUtcs = new UniTaskCompletionSource<IEnumerable<ImmutableLine>>();
        
        public async UniTask<IEnumerable<ImmutableLine>> DrawLines()
        {
            isDrawing.Value = true;
            var lines = await onLineDrawFinishedUtcs.Task;
            isDrawing.Value = false;
            return lines;
        }

        
        private readonly BoolReactiveProperty isDrawing = new BoolReactiveProperty(false);
        public BoolReactiveProperty CanDraw() => isDrawing;
        
        public Action<IEnumerable<ImmutableLine>> OnLineDrawFinished()
        {
            return lines => onLineDrawFinishedUtcs.TrySetResult(lines);
        }

        public LineDrawController()
        {
            
        }

        public void Dispose()
        {
            isDrawing?.Dispose();
        }
    }
    
    public sealed class TestMono : LifetimeScope
    {
        [SerializeField] private LifetimeScope lineDrawLifetimeScope = default;
        private DrawnLineModel _drawnLineModel;

        [SerializeField] private LifetimeScope imageLoadLifetimeScope = default;

        private LineDrawController lineDrawController = new LineDrawController();
        [SerializeField] private Button button;
        
        private void Start()
        {
            // 最初は画像の読み込み
            // 読み込みが終わったら、読み込み用UIを見えなくする。
            // 画像が表示されるので、ラインを引くモードにする
            // ラインが引かれたら、GenerateButtonを押してモデルをだす。

            button.OnClickAsObservable()
                .Subscribe(_ => lineDrawController.CanDraw().Value = false)
                .AddTo(this);
        }

        protected override void Awake()
        {
            base.Awake();

            // TODO: 親子関係の作り方。ImageLoaderのPrefab化 + EnqueueParentをつかって、ImageLoader側のIContainerBuilderにIImageShowerを登録してやるのが綺麗なやり方かも。検討する。
            imageLoadLifetimeScope.parentReference.Object = this;
            lineDrawLifetimeScope.parentReference.Object = this;
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

    public sealed class SequenceController : IAsyncStartable
    {
        private readonly ImageShower imageShower;
        private readonly LineDrawController lineDrawController;
        
        [Inject]
        public SequenceController(ImageShower imageShower, LineDrawController lineDrawController)
        {
            this.imageShower = imageShower;
            this.lineDrawController = lineDrawController;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // FIX: 表示方法もこちらから指定するのが自然な気がする。UIの非表示もこっちで指示できるし。とりあえず後回しでいいけど後でなおす。
            await imageShower.LoadImage();
            
            var lines = await lineDrawController.DrawLines();
            
            Debug.Log($"Lines: {lines.Count()}");
            
            // var houseModel = await modelGenerator.Generate(lines);
            
            // 次のシーン読みこみ？ + houseModelの引き渡し？
        }
    }
}