using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MadoriVR.Scripts.CreateHouseModel.LineDrawing;
using UniRx;

namespace MadoriVR.Scripts.CreateHouseModel.Core
{
    public sealed class LineDrawController : ILineDrawClient, IDisposable
    {
        private readonly UniTaskCompletionSource<IEnumerable<ImmutableLine>> onLineDrawFinishedUtcs = new();
        
        public async UniTask<IEnumerable<ImmutableLine>> DrawLines()
        {
            isDrawing.Value = true;
            var lines = await onLineDrawFinishedUtcs.Task;
            isDrawing.Value = false;
            return lines;
        }

        
        private readonly BoolReactiveProperty isDrawing = new (false);
        public BoolReactiveProperty CanDraw() => isDrawing;
        
        public Action<IEnumerable<ImmutableLine>> OnLineDrawFinished()
        {
            return lines => onLineDrawFinishedUtcs.TrySetResult(lines);
        }

        public void Dispose()
        {
            isDrawing?.Dispose();
        }
    }
}