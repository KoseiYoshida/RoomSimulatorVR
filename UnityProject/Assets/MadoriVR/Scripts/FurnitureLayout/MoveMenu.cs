using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public enum RotationAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
    }
    
    public sealed class MoveMenu : MonoBehaviour
    {
        [SerializeField] private Button[] buttons = new Button[6];

        private readonly Subject<MoveCommand> commandSubject = new();
        public IObservable<MoveCommand> OnCommand => commandSubject;

        [Header("x, y, zの順番")]
        [SerializeField] private Slider[] rotationSliders = new Slider[3];
        private readonly ReactiveProperty<(RotationAxis, float)> angleSubject = new();
        public IReadOnlyReactiveProperty<(RotationAxis axis, float angle)> OnAngleChanged => angleSubject;

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

            for (int i = 0; i < rotationSliders.Length; i++)
            {
                var axis = (RotationAxis) i;
                rotationSliders[i].OnValueChangedAsObservable()
                    .Subscribe(value => angleSubject.Value = (axis, value))
                    .AddTo(this);
            }
        }

        public void ChangeInteractable(bool interactable)
        {
            foreach (var button in buttons)
            {
                button.interactable = interactable;
            }

            foreach (var slider in rotationSliders)
            {
                slider.interactable = interactable;
            }
        }

        public void SetRotationValue(Vector3 angle)
        {
            rotationSliders[0].SetValueWithoutNotify(angle.x);
            rotationSliders[1].SetValueWithoutNotify(angle.x);
            rotationSliders[2].SetValueWithoutNotify(angle.x);
        }
    }
}