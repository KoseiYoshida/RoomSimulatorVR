using System;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class Mover : MonoBehaviour, IMovable
    {
        [SerializeField] private Transform rotationPivot = default;
        
        public void ChangePosition(MoveCommand command)
        {
            Vector3 diff = command switch
            {
                MoveCommand.XPlus => Vector3.right,
                MoveCommand.XMinus => Vector3.left,
                MoveCommand.YPlus => Vector3.up,
                MoveCommand.YMinus => Vector3.down,
                MoveCommand.ZPlus => Vector3.forward,
                MoveCommand.ZMinus => Vector3.back,
                _ => throw new ArgumentOutOfRangeException(),
            };
            
            transform.localPosition += diff;
        }

        public void ChangeRotation(RotationAxis axis, float angle)
        {
            var currentCopy = rotationPivot.localRotation.eulerAngles;
            switch(axis)
            {
                case RotationAxis.X: 
                    currentCopy.x = angle;
                    break;
                case RotationAxis.Y:
                    currentCopy.y = angle;
                    break;
                case RotationAxis.Z: 
                    currentCopy.z = angle;
                    break;
            }
            
            rotationPivot.localRotation = Quaternion.Euler(currentCopy);
        }

        public Vector3 GetRotation()
        {
            return transform.localEulerAngles;
        }
    }
}