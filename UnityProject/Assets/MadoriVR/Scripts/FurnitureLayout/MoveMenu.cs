using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public enum MoveCommand
    {
        XPlus,
        XMinus,
        YPlus,
        YMinus,
        ZPlus,
        ZMinus,
    }
    
    public sealed class MoveMenu : MonoBehaviour
    {
        [SerializeField] private Button[] buttons = new Button[6];

        private readonly Subject<MoveCommand> commandSubject = new();
        public IObservable<MoveCommand> OnCommand => commandSubject;

        [SerializeField] private Slider rotationSlider = default;
        private readonly Subject<float> angleSubject = new();
        public IObservable<float> OnAngle => angleSubject;

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
                .Subscribe(value => angleSubject.OnNext(value))
                .AddTo(this);
        }

        public void ChangeValidity(bool isValid)
        {
            foreach (var button in buttons)
            {
                button.interactable = isValid;
            }

            rotationSlider.interactable = isValid;
        }

        public void SetRotationValue(float angle)
        {
            rotationSlider.SetValueWithoutNotify(angle);
        }
    }
}