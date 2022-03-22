using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public interface IMovable
    {
        void ChangePosition(MoveCommand command);
        void ChangeRotation(RotationAxis axis, float angle);
        Vector3 GetRotation();
    }
}