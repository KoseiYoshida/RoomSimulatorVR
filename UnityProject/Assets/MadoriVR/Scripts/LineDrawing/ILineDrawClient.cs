using System;
using System.Collections.Generic;
using UniRx;

namespace MadoriVR.Scripts.LineDrawing
{
    public interface ILineDrawClient
    {
        // TODO: アクセス制限を適切にかける。
        public BoolReactiveProperty CanDraw();
        public Action<IEnumerable<ImmutableLine>> OnLineDrawFinished();
    }
}