using System;
using UniRx;
using UnityEngine;

namespace MadoriVR
{
    public sealed class ClickPointProvider : MonoBehaviour
    {
        // FIX: Vector2そのままじゃない方がいいかも。(x, y) -> (x, z)となったりがややこしいので。
        private readonly Subject<Vector2> clickPosSubject = new Subject<Vector2>();
        public IObservable<Vector2> OnClicked => clickPosSubject;
        
        private Camera mainCameraCache;
        
        private void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => GetHitPos())
                .Subscribe(value =>
                {
                    if (value.HasValue)
                    {
                        clickPosSubject.OnNext(value.Value);
                    }
                })
                .AddTo(this);
            

            mainCameraCache = Camera.main;
        }

        private Vector2? GetHitPos()
        {
            if (Physics.Raycast(mainCameraCache.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                return new Vector2(hit.point.x, hit.point.z);
            }
            else
            {
                return null;
            }
        }
    }
}