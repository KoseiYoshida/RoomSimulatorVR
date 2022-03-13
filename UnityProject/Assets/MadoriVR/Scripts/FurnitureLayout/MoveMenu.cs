using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class MoveMenu : MonoBehaviour
    {
        [SerializeField] private Button[] buttons = new Button[6];

        private readonly Subject<MoveCommand> commandSubject = new();
        public IObservable<MoveCommand> OnCommand => commandSubject;

        [SerializeField] private Slider rotationSlider = default;
        private readonly ReactiveProperty<float> angleSubject = new();
        public IReadOnlyReactiveProperty<float> OnAngle => angleSubject;

        private void Start()
        {
            commandSubject.AddTo(this);
            for (int i = 0; i < buttons.Length; i++)
            {
                var command = (MoveCommand) i;
                buttons[i].OnClickAsObservable()
                    .Subscribe(_ => commandSubject.OnNext(command))
                    .AddTo(this);
            }

            angleSubject.AddTo(this);
            rotationSlider.OnValueChangedAsObservable()
                .Subscribe(value => angleSubject.Value = value)
                .AddTo(this);
        }

        public void ChangeInteractable(bool interactable)
        {
            foreach (var button in buttons)
            {
                button.interactable = interactable;
            }

            rotationSlider.interactable = interactable;
        }

        public void SetRotationValue(float angle)
        {
            rotationSlider.SetValueWithoutNotify(angle);
        }
    }
}