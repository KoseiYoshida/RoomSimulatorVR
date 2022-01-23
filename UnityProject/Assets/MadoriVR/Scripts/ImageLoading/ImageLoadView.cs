using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.ImageLoading
{
    public sealed class ImageLoadView : MonoBehaviour, IImageSelector, IImageShower
    {
        [SerializeField] private InputField pathInputField = default;
        [SerializeField] private Button loadButton = default;

        [SerializeField] private RawImage rawImage = default;
        [SerializeField] private Text message = default;
        

        private readonly Subject<string> loadPathSubject = new Subject<string>();
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

        public void ShowImage(Texture2D texture)
        {
            rawImage.texture = texture;
            var size = new Vector2(texture.width, texture.height);
            var scaleXY = size / 1000f;
            // FIX: サイズ調整を適切にする。この調整はPresenter側が指示を出すのが適切かも。
            scaleXY *= 3;
            rawImage.transform.localScale = new Vector3(scaleXY.x, scaleXY.y, 1.0f);
        }

        public void ShowLoadResult(string text)
        {
            message.text = text;
        }
    }
}