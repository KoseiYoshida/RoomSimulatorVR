using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    [RequireComponent(typeof(Furniture))]
    [RequireComponent(typeof(SelectedStatus))]
    public sealed class SelectedLooksChanger : MonoBehaviour
    {
        [SerializeField] private GameObject selectedEffectBox = default;
        private Renderer targetRenderer;

        private void Start()
        {
            targetRenderer = selectedEffectBox.GetComponent<Renderer>();
            
            var furniture = GetComponent<Furniture>();
            furniture.FurnitureBounds
                .Subscribe(value =>
                {
                    selectedEffectBox.transform.localPosition = value.center;
                    selectedEffectBox.transform.localScale = value.size;
                }).AddTo(this);
                
            var selectedStatus = GetComponent<SelectedStatus>();
            selectedStatus.IsSelected
                .Subscribe(value => targetRenderer.enabled = value)
                .AddTo(this);
        }
    }
}