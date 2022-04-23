using System;
using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class CameraMoveEventProvider : MonoBehaviour
    {
        private const int MouseLeftClickButtonIndex = 0;
        private const int MouseRightClickButtonIndex = 1;
        private const int MouseWheelButtonIndex = 2;
        
        private readonly Subject<float> onZoomChanged = new();
        /// <summary>
        /// -1.0(zoom down) ~ 1.0(zoom up)
        /// </summary>
        public IObservable<float> OnZoomChanged => onZoomChanged;
    
        private readonly Subject<Vector3> onCameraParallelDisplacement = new();
        public IObservable<Vector3> OnCameraParallelDisplacement => onCameraParallelDisplacement;
        private Vector3? mousePositionCache;

        private readonly Subject<Vector2> onCameraRotationDisplacement = new();
        public IObservable<Vector2> OnCameraRotationDisplacement => onCameraRotationDisplacement;
        private Vector2? rotationStartPosition;

        private void Start()
        {
            onZoomChanged.AddTo(this);
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    // 前方に向かう回転でズームアップにしたいので-1をかけている。
                    onZoomChanged.OnNext(-1 * Input.mouseScrollDelta.y);
                })
                .AddTo(this);

            onCameraParallelDisplacement.AddTo(this);
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButton(MouseWheelButtonIndex))
                    {
                        if (mousePositionCache is { } notNullValue)
                        {
                            // UnityEditorの挙動に合わせたいので-1をかけている。
                            onCameraParallelDisplacement.OnNext(-1 * (Input.mousePosition - notNullValue));
                        }

                        mousePositionCache = Input.mousePosition;
                    }
                    else
                    {
                        mousePositionCache = null;
                    }
                }).AddTo(this);

            onCameraRotationDisplacement.AddTo(this);
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButton(MouseRightClickButtonIndex))
                    {
                        if (rotationStartPosition is { } notNullValue)
                        {
                            onCameraRotationDisplacement.OnNext((Vector2)Input.mousePosition - notNullValue);
                        }
                        
                        rotationStartPosition = Input.mousePosition;
                    }
                    else
                    {
                        rotationStartPosition = null;
                    }
                }).AddTo(this);
        }
    }
}