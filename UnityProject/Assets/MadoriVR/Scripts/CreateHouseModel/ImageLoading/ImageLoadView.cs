using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.CreateHouseModel.ImageLoading
{
    public sealed class ImageLoadView : MonoBehaviour, IImageSelector
    {
        [SerializeField] private InputField pathInputField = default;
        [SerializeField] private Button loadButton = default;
        
        private readonly Subject<string> loadPathSubject = new();
        public IObservable<string> OnPathEntered() => loadPathSubject;
        
        private void Start()
        {
            pathInputField.OnValueChangedAsObservable()
                .Subscribe(text =>
                {
                    loadButton.interactable = !string.IsNullOrWhiteSpace(text);
                }).AddTo(this);

            loadButton.OnClickAsObservable()
                .Subscribe(_ => loadPathSubject.OnNext(pathInputField.text))
                .AddTo(this);
        }
    }
}