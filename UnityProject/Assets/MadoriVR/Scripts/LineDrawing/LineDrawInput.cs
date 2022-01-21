using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.LineDrawing
{
    public sealed class LineDrawInput : IStartable, IDisposable
    {
        private readonly DrawLineModel model;
        private readonly LineDrawEventProvider lineDrawEventProvider;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        private bool isDrawing;
        private IDisposable mouseHitDisposable; 

        [Inject]
        public LineDrawInput(DrawLineModel model, LineDrawEventProvider lineDrawEventProvider)
        {
            this.model = model;
            this.lineDrawEventProvider = lineDrawEventProvider;
        }
        
        public void Start()
        {
            lineDrawEventProvider.OnClicked
                .Subscribe(point =>
                {
                    if (isDrawing)
                    {
                        var drawing = model.DrawingLine.Value;
                        var line = new ImmutableLine(drawing.Point1, point);
                        model.AddLine(drawing);

                        isDrawing = false;
                        StopObservingMouseHit();
                    }
                    else
                    {
                        model.ChangeDrawingLine(new ImmutableLine(point, point));

                        isDrawing = true;
                        StartObservingMouseHit();
                    }
                })
                .AddTo(compositeDisposable);

            void StartObservingMouseHit()
            {
                mouseHitDisposable = lineDrawEventProvider.OnMouseHit
                    .Subscribe(point =>
                    {
                        var drawing = model.DrawingLine.Value;
                        
                        // FIX: 毎回インスタンスをつくりなおすのではなく、更新と通知ができるようにしておく。
                        var line = new ImmutableLine(drawing.Point1, point);
                        model.ChangeDrawingLine(line);
                    })
                    .AddTo(compositeDisposable);
            }

            void StopObservingMouseHit()
            {
                mouseHitDisposable.Dispose();
            }
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}