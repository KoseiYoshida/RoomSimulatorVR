using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.LineDrawing
{
    public sealed class LineSupplier : IStartable, IDisposable
    {
        private readonly DrawnLineModel model;
        private readonly ILineDrawClient lineDrawClient;

        private readonly CompositeDisposable compositeDisposable = new();

        [Inject]
        public LineSupplier(DrawnLineModel model, ILineDrawClient lineDrawClient)
        {
            this.model = model;
            this.lineDrawClient = lineDrawClient;
        }

        public void Start()
        {
            lineDrawClient.CanDraw()
                .SkipLatestValueOnSubscribe()
                .Where(value => !value)
                .Subscribe(_ =>
                {
                    lineDrawClient.OnLineDrawFinished().Invoke(model.Lines);
                }).AddTo(compositeDisposable);
        }


        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}