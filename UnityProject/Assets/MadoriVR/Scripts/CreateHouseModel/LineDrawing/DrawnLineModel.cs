using UniRx;

namespace MadoriVR.Scripts.CreateHouseModel.LineDrawing
{
    /// <summary>
    /// Hold drawn line informations. 
    /// </summary>
    public sealed class DrawnLineModel
    {
        private readonly ReactiveCollection<ImmutableLine> lines = new ReactiveCollection<ImmutableLine>();
        /// <summary>
        /// Drawn lines.
        /// </summary>
        public IReactiveCollection<ImmutableLine> Lines => lines;

        private readonly ReactiveProperty<ImmutableLine> drawingLine = new ReactiveProperty<ImmutableLine>();
        /// <summary>
        /// Being Drawn line.
        /// </summary>
        public IReadOnlyReactiveProperty<ImmutableLine> DrawingLine => drawingLine;

        
        /// <summary>
        /// Add drawn line.
        /// </summary>
        /// <param name="immutableLine"></param>
        public void AddLine(ImmutableLine immutableLine)
        {
            lines.Add(immutableLine);
        }

        /// <summary>
        /// Change drawing line.
        /// </summary>
        /// <param name="immutableLine"></param>
        public void ChangeDrawingLine(ImmutableLine immutableLine)
        {
            drawingLine.Value = immutableLine;
        }
    }
}