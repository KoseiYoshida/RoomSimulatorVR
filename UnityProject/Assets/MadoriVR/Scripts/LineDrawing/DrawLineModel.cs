using UniRx;

namespace MadoriVR.Scripts.LineDrawing
{
    public sealed class DrawLineModel
    {
        private readonly ReactiveCollection<ImmutableLine> lines = new ReactiveCollection<ImmutableLine>();
        public IReactiveCollection<ImmutableLine> Lines => lines;

        private readonly ReactiveProperty<ImmutableLine> drawingLine = new ReactiveProperty<ImmutableLine>();
        public IReadOnlyReactiveProperty<ImmutableLine> DrawingLine => drawingLine; 

        public void AddLine(ImmutableLine immutableLine)
        {
            lines.Add(immutableLine);
        }

        public void ChangeDrawingLine(ImmutableLine immutableLine)
        {
            drawingLine.Value = immutableLine;
        }
    }
}