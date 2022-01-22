using NUnit.Framework;
using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.LineDrawing.Tests
{
    public sealed class DrawLineModelTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CanReceiveAddedLine()
        {
            var model = new DrawLineModel();
            ImmutableLine line = new ImmutableLine(Vector2.one, Vector2.one * 2);
            ImmutableLine added = null;
            model.Lines.ObserveAdd()
                .Subscribe(addEvent => added = addEvent.Value);
            
            model.AddLine(line);
            
            Assert.That(added, Is.EqualTo(line));
        }

        [Test]
        public void CanReceiveDrawingLine()
        {
            var model = new DrawLineModel();
            ImmutableLine line = new ImmutableLine(Vector2.one, Vector2.one * 2);
            ImmutableLine drawing = null;
            model.DrawingLine
                .SkipLatestValueOnSubscribe()
                .Subscribe(value => drawing = value);
            
            model.ChangeDrawingLine(line);
            
            Assert.That(drawing, Is.EqualTo(line));
        }
    }
}
