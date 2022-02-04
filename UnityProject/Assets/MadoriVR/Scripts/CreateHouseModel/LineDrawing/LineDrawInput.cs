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

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();
        private CompositeDisposable drawingDisposable;

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
            // FIX: クラスが複雑になりすぎ。分離する。
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
        }

        private void EnableDrawing()
        {
            drawingDisposable = new CompositeDisposable();
            
            lineDrawEventProvider.OnClicked
                .Subscribe(point =>
                {
                    if (isDrawingALine)
                    {
                        var drawing = model.DrawingLine.Value;
                        var line = new ImmutableLine(drawing.point1, point);
                        model.AddLine(drawing);

                        isDrawingALine = false;
                        StopObservingMouseHit();
                    }
                    else
                    {
                        model.ChangeDrawingLine(new ImmutableLine(point, point));

                        isDrawingALine = true;
                        StartObservingMouseHit();
                    }
                })
                .AddTo(drawingDisposable);

            void StartObservingMouseHit()
            {
                mouseHitDisposable = lineDrawEventProvider.OnMouseHit
                    .Subscribe(point =>
                    {
                        var drawing = model.DrawingLine.Value;
                        
                        // FIX: 毎回インスタンスをつくりなおすのではなく、更新と通知ができるようにしておく。
                        var line = new ImmutableLine(drawing.point1, point);
                        model.ChangeDrawingLine(line);
                    })
                    .AddTo(drawingDisposable);
            }

            void StopObservingMouseHit()
            {
                mouseHitDisposable.Dispose();
            }
        }

        private void DisableDrawing()
        {
            drawingDisposable?.Dispose();
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}