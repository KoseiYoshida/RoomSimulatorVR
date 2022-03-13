using System;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class Mover : MonoBehaviour, IMovable
    {
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

        public void ChangeRotation(float angle)
        {
            var current = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(current.x, angle, current.z);
        }

        public float GetRotation()
        {
            return transform.localEulerAngles.y;
        }
    }
}