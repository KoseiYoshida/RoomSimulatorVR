using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.LineDrawing
{
    public sealed class LineDrawPresenter : IStartable, IDisposable
    {
        // FIX: 今は簡単のために高さを決め打ちにしてる。状況に合わせて設定できる方が自然。
        private (Vector3, Vector3) To3DPos(ImmutableLine immutableLine)
        {
            const float Height = 1.0f;
            
            return 
            (
                new Vector3(immutableLine.Point1.x, Height, immutableLine.Point1.y),
                new Vector3(immutableLine.Point2.x, Height, immutableLine.Point2.y) 
            );
        }

        private readonly DrawLineModel model;
        private readonly LineDrawView drawView;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        // FIX: indexをValueObject化する。
        private (bool isDrawing, int lineIndex) drawState = (false, int.MinValue);

        [Inject]
        public LineDrawPresenter(DrawLineModel model, LineDrawView drawView)
        {
            this.model = model;
            this.drawView = drawView;
        }

        public void Start()
        {
            model.Lines.ObserveAdd()
                .Select(e => e.Value)
                .Select(To3DPos)
                .Subscribe(value =>
                {
                    // 書き途中のものを消す
                    if (drawState.isDrawing)
                    {
                        drawView.DeleteLine(drawState.lineIndex);
                        drawState = (false, int.MinValue);
                    }

                    // 新しく書く
                    drawView.DrawLine(value);
                }).AddTo(compositeDisposable);

            model.DrawingLine
                .Where(value => value != null)
                .Select(To3DPos)
                .Subscribe(value =>
                {
                    // 書き途中の線があるなら更新、無いなら新しくつくる。
                    if (drawState.isDrawing)
                    {
                        drawView.MoveLine(drawState.lineIndex, value);
                    }
                    else
                    {
                        var index = drawView.DrawLine(value);
                        drawState = (true, index);
                    }

                }).AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}