using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class Furniture : MonoBehaviour
    {
        [SerializeField] private GameObject raycastTargetCube;
        private readonly ReactiveProperty<Bounds> furnitureBounds = new(new Bounds(Vector3.one, Vector3.one));
        public IReadOnlyReactiveProperty<Bounds> FurnitureBounds => furnitureBounds;

        private void Awake()
        {
            furnitureBounds.AddTo(this);
            
            furnitureBounds.SkipLatestValueOnSubscribe()
                .Subscribe(value =>
                {
                    raycastTargetCube.transform.localPosition = value.center;
                    raycastTargetCube.transform.localScale = value.size;
                }).AddTo(this);
        }

        public void Initialize(GameObject prefab)
        {
            name = $"Parent-{prefab.name}";
            var furnitureModel = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            RecalculateBounds(furnitureModel);
        }
        
        private void RecalculateBounds(GameObject furnitureModel)
        {
            var bounds = new Bounds();
            var renderers = furnitureModel.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                bounds.Encapsulate(r.bounds);
            }

            furnitureBounds.Value = bounds;
        }
    }
}