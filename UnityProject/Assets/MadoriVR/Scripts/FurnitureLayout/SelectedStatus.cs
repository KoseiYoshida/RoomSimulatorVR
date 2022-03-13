using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class SelectedStatus : MonoBehaviour, ISelectMessageHandler
    {
        private readonly BoolReactiveProperty isSelected = new();
        public IReadOnlyReactiveProperty<bool> IsSelected => isSelected;

        private void Start()
        {
            isSelected.AddTo(this);
        }

        public void Select()
        {
            isSelected.Value = true;
        }

        public void Unselect()
        {
            isSelected.Value = false;
        }
    }
}