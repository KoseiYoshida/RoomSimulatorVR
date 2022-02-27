using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class PositionSender : MonoBehaviour
    {
        private Camera mainCameraCache;

        [SerializeField] private MoveMenu menu;

        private Mover currentSelectMover;
        
        private void Awake()
        {
            mainCameraCache = Camera.main;
        }

        private void Start()
        {
            menu.ChangeValidity(false);
            menu.OnCommand.Subscribe(ChangeMoverPos).AddTo(this);
            menu.OnAngle.Subscribe(ChangeRotation).AddTo(this);
            
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(value =>
                {
                    if (Physics.Raycast(mainCameraCache.ScreenPointToRay(Input.mousePosition), out var hit))
                    {
                        if (hit.collider.gameObject.TryGetComponent<Mover>(out var mover))
                        {
                            currentSelectMover = mover;
                            Debug.Log($"Selected : {mover.gameObject.name}");
                            menu.ChangeValidity(true);
                            menu.SetRotationValue(currentSelectMover.transform.localEulerAngles.y);
                        }
                        else
                        {
                            menu.ChangeValidity(false);
                            currentSelectMover = null;
                        }
                    }
                })
                .AddTo(this);
        }

        private void ChangeMoverPos(MoveCommand command)
        {
            if (currentSelectMover == null)
            {
                return;
            }
            
            Vector3 diff;
            switch (command)
            {
                case MoveCommand.XPlus: diff = Vector3.right; break;
                case MoveCommand.XMinus: diff = Vector3.left; break;
                case MoveCommand.YPlus: diff = Vector3.up; break;
                case MoveCommand.YMinus: diff = Vector3.down; break;
                case MoveCommand.ZPlus: diff = Vector3.forward; break;
                case MoveCommand.ZMinus: diff = Vector3.back; break;
                default: throw new NotImplementedException();
            }
            currentSelectMover.transform.localPosition += diff;
        }

        private void ChangeRotation(float angle)
        {
            if (currentSelectMover == null)
            {
                return;
            }
            
            var current = currentSelectMover.transform.localEulerAngles;
            currentSelectMover.transform.localEulerAngles = new Vector3(current.x, angle, current.z);
        }
    }
}