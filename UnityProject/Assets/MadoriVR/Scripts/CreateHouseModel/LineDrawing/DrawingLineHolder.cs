using System;
using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.CreateHouseModel.LineDrawing
{
    public sealed class DrawingLineHolder : IDisposable
    {
        private BoolReactiveProperty isDrawing = new();
        public IReadOnlyReactiveProperty<bool> IsDrawing => isDrawing;

        private readonly ReactiveProperty<ImmutableLine> inCompleteLine = new();
        public IReadOnlyReactiveProperty<ImmutableLine> IncompleteLine => inCompleteLine;

        private readonly ReactiveProperty<ImmutableLine> completeLine = new();
        public IReadOnlyReactiveProperty<ImmutableLine> CompleteLine => completeLine; 
        
        public void AddPoint(Vector2 point)
        {
            if (isDrawing.Value)
            {
                var drawing = inCompleteLine.Value;
                var line = new ImmutableLine(drawing.point1, point);
                completeLine.Value = line;

                isDrawing.Value = false;
            }
            else
            {
                inCompleteLine.Value = new ImmutableLine(point, point);

                isDrawing.Value = true;
            }
        }

        public void ChangeDrawingLinePoint2(Vector2 newPoint2)
        {
            var current = inCompleteLine.Value;
            inCompleteLine.Value = new ImmutableLine(current.point1, newPoint2);
        }

        public void Dispose()
        {
            isDrawing?.Dispose();
            inCompleteLine?.Dispose();
            completeLine?.Dispose();
        }
    }
}