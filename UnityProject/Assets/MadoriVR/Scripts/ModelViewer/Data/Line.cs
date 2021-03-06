using System;
using UnityEngine;

namespace MadoriVR.Scripts.ModelViewer.Data
{
    /// <summary>
    /// A line.
    /// </summary>
    public sealed class Line : IEquatable<Line>
    {
        public readonly Vector2 point1;
        public readonly Vector2 point2;

        public Line(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public bool Equals(Line other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return point1.Equals(other.point1) && point2.Equals(other.point2);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Line other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (point1.GetHashCode() * 397) ^ point2.GetHashCode();
            }
        }

        public static bool operator ==(Line left, Line right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Line left, Line right)
        {
            return !Equals(left, right);
        }
    }
}