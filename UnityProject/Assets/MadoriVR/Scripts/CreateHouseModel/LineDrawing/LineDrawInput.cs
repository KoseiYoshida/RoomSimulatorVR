using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.LineDrawing
{
    /// <summary>
    /// Update <see cref="DrawnLineModel"/> by observing <see cref="lineDrawEventProvider"/>.
    /// </summary>
    public sealed class LineDrawInput : IStartable, IDisposable
    {
        private readonly DrawnLineModel model;
        private readonly LineDrawEventProvider lineDrawEventProvider;
        private readonly ILineDrawClient drawClient;

        private readonly CompositeDisposable compositeDisposable = new();
        private CompositeDisposable drawingDisposable;

        private readonly DrawingLineHolder drawingLineHolder = new ();

        private bool isDrawingALine;
        private IDisposable mouseHitDisposable; 

        [Inject]
        public LineDrawInput(DrawnLineModel model, LineDrawEventProvider lineDrawEventProvider, ILineDrawClient drawClient)
        {
            this.model = model;
            this.lineDrawEventProvider = lineDrawEventProvider;
            this.drawClient = drawClient;
        }
        
        public void Start()
        {
            drawClient.CanDraw()
                .Subscribe(value =>
                {
                    if (value)
                    {
                        EnableDrawing();
                    }
                    else
                    {
                        DisableDrawing();
                    }

                }).AddTo(compositeDisposable);

            drawingLineHolder.IsDrawing
                .Subscribe(value =>
                {
                    if (value)
                    {
                        StartObservingMouseHit();
                    }
                    else
                    {
                        StopObservingMouseHit();
                    }
                }).AddTo(compositeDisposable);

            drawingLineHolder.IncompleteLine
                .SkipLatestValueOnSubscribe()
                .Subscribe(model.ChangeDrawingLine)
                .AddTo(compositeDisposable);

            drawingLineHolder.CompleteLine
                .SkipLatestValueOnSubscribe()
                .Subscribe(model.AddLine)
                .AddTo(compositeDisposable);
        }

        private void EnableDrawing()
        {
            drawingDisposable = new CompositeDisposable();
            
            lineDrawEventProvider.OnClicked
                .Subscribe(point =>
                {
                    drawingLineHolder.AddPoint(point);
                })
                .AddTo(drawingDisposable);
        }
        
        private void DisableDrawing()
        {
            drawingDisposable?.Dispose();
        }
        
        private void StartObservingMouseHit()
        {
            // OnMouseHitは処理負荷軽減のため購読の解除を行う必要あり。そのためWhereオペレータなどで処理を切り替えるのではなく、購読/解除で処理を切り替える。
            mouseHitDisposable = lineDrawEventProvider.OnMouseHit
                .Subscribe(drawingLineHolder.ChangeDrawingLinePoint2)
                .AddTo(drawingDisposable);
        }

        private void StopObservingMouseHit()
        {
            mouseHitDisposable?.Dispose();
        }
        
        public void Dispose()
        {
            compositeDisposable?.Dispose();
            drawingDisposable?.Dispose();
            drawingLineHolder?.Dispose();
        }
    }
}