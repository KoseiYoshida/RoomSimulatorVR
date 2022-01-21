using System;
using UnityEngine;

namespace MadoriVR.Scripts.LineDrawing
{
    public sealed class ImmutableLine : IEquatable<ImmutableLine>
    {
        public readonly Vector2 Point1;
        public readonly Vector2 Point2;

        public ImmutableLine(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public bool Equals(ImmutableLine other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Point1.Equals(other.Point1) && Point2.Equals(other.Point2);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ImmutableLine other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Point1.GetHashCode() * 397) ^ Point2.GetHashCode();
            }
        }

        public static bool operator ==(ImmutableLine left, ImmutableLine right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ImmutableLine left, ImmutableLine right)
        {
            return !Equals(left, right);
        }
    }
}