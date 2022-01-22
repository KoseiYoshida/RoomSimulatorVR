using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.LineDrawing
{
    public sealed class LineDrawEventProvider : MonoBehaviour
    {
        private Camera mainCameraCache;

        // FIX: Vector2そのままじゃない方がいいかも。(x, y) -> (x, z)となったりがややこしいので。
        // FIX: Raycastしてるなら、そのまま3D座標送ってやった方がわかりやすいそう。
        private readonly Subject<Vector2> clickHitPosSubject = new Subject<Vector2>();
        public IObservable<Vector2> OnClicked => clickHitPosSubject;

        private readonly Subject<Vector2> mouseHitPosSubject = new Subject<Vector2>();
        /// <summary>
        /// Publish positions which be hit by raycasts from mouse pointing points.
        /// </summary>
        /// <remarks>Publish only <see cref="mouseHitPosSubject"/> has subscriber.</remarks>
        public IObservable<Vector2> OnMouseHit => mouseHitPosSubject;
        private IEnumerator publishMouseHitPointCoroutine;

        
        private void Start()
        {
            mainCameraCache = Camera.main;

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => GetHitPos())
                .Subscribe(value =>
                {
                    if (value.HasValue)
                    {
                        clickHitPosSubject.OnNext(value.Value);
                    }
                })
                .AddTo(this);

            publishMouseHitPointCoroutine = PublishDragPointContinuously();
            StartCoroutine(publishMouseHitPointCoroutine);
        }

        private void OnDestroy()
        {
            if (publishMouseHitPointCoroutine != null)
            {
                StopCoroutine(publishMouseHitPointCoroutine);
            }
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

        private IEnumerator PublishDragPointContinuously()
        {
            var wait = new WaitForEndOfFrame();

            while (true)
            {
                if (mouseHitPosSubject.HasObservers)
                {
                    // FIX: Mouseが対象にのってないときや、画面外にあるときは処理を省略する？
                    
                    var hitPos = GetHitPos();
                    if (hitPos.HasValue)
                    {
                        mouseHitPosSubject.OnNext(hitPos.Value);
                    }
                    else
                    {
                        Debug.LogWarning("Ray didn't hit anything.");
                    }
                }
                
                yield return wait;
            }
        }
    }
}