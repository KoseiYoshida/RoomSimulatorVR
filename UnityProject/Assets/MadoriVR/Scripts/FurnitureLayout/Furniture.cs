using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class Furniture : MonoBehaviour
    {
        [SerializeField] private GameObject raycastTargetCube = default;
        [SerializeField] private GameObject adjuster = default;
        
        private readonly ReactiveProperty<Bounds> furnitureBounds = new(new Bounds(Vector3.one, Vector3.one));
        public IReadOnlyReactiveProperty<Bounds> FurnitureBounds => furnitureBounds;

        private void Awake()
        {
            furnitureBounds.AddTo(this);
            
            furnitureBounds.SkipLatestValueOnSubscribe()
                .Subscribe(value =>
                {
                    // Adjusterのこどもなので位置は自動調整される。
                    // raycastTargetCube.transform.localPosition = value.center;
                    raycastTargetCube.transform.localScale = value.size;
                }).AddTo(this);
        }

        public void Initialize(GameObject prefab)
        {
            name = $"Parent-{prefab.name}";
            
            var parentTransform = adjuster.transform;
            var furnitureModel = Instantiate(prefab, Vector3.zero, Quaternion.identity, parentTransform);
            parentTransform.localPosition = prefab.transform.localPosition;
            parentTransform.localRotation = prefab.transform.localRotation;

            var bounds = RecalculateBounds(furnitureModel);
            furnitureBounds.Value = bounds;
            
            // 空間とObjectの回転関係合わせるために180°まわす。
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
        private Bounds RecalculateBounds(GameObject furnitureModel)
        {
            var bounds = new Bounds();
            var renderers = furnitureModel.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                bounds.Encapsulate(r.bounds);
            }

            return bounds;
        }
    }
}