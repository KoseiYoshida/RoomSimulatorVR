using MadoriVR.Scripts.LineDrawing;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouse
{
    public sealed class Temp : MonoBehaviour
    {
        [SerializeField] private LifetimeScope lineDrawLifetimeScope = default;
        private DrawnLineModel _drawnLineModel;

        [SerializeField] private Button generateButton = default;
    
        private void Start()
        {
            _drawnLineModel = (DrawnLineModel)lineDrawLifetimeScope.Container.Resolve(typeof(DrawnLineModel));

            generateButton.OnClickAsObservable()
                .Subscribe(_ => Create())
                .AddTo(this);
        }

        private void Create()
        {
            var parent = new GameObject("Room");
            
            foreach (var line in _drawnLineModel.Lines)
            {
                var start = line.point1;
                var end = line.point2;

                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var wallTransform = wall.transform;
                wallTransform.SetParent(parent.transform);

                var width = Mathf.Abs(end.x - start.x);
                var depth = Mathf.Abs(end.y - start.y);

                // 壁に適度な厚みをもたせる
                const float MIN_THICKNESS = 3;
                const float HEIGHT = 3;
                wallTransform.localScale = new Vector3(
                    Mathf.Max(width, MIN_THICKNESS), 
                    HEIGHT, 
                    Mathf.Max(depth, MIN_THICKNESS)
                );

                const float POS_Y = 1.0f;
                wallTransform.localPosition = new Vector3(
                    Mathf.Min(start.x, end.x) + width / 2, 
                    POS_Y, 
                    Mathf.Min(start.y, end.y) + depth / 2
                );
            }

        }
    }
}