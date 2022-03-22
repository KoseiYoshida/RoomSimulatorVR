using UniRx;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class MenuController : MonoBehaviour
    {
        [SerializeField] private Selector selector;
        [SerializeField] private MoveMenu menu = default;

        private IMovable targetMovable;
        
        private void Start()
        {
            selector.SelectedObject
                .SkipLatestValueOnSubscribe()
                .Subscribe(value =>
                {
                    if (value != null && value.TryGetComponent<IMovable>(out var movable))
                    {
                        targetMovable = movable;
                        menu.ChangeInteractable(true);
                        menu.SetRotationValue(targetMovable.GetRotation());
                    }
                    else
                    {
                        menu.ChangeInteractable(false);
                        targetMovable = null;
                    }
                }).AddTo(this);
            
            menu.ChangeInteractable(false);
            menu.OnCommand
                .Subscribe(value => targetMovable.ChangePosition(value)).AddTo(this);
            menu.OnAngleChanged
                .SkipLatestValueOnSubscribe()
                .Subscribe(value => targetMovable.ChangeRotation(value.axis, value.angle)).AddTo(this);
        }
    }
}