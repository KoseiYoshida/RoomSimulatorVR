using Cysharp.Threading.Tasks;
using MadoriVR.Scripts.CreateHouseModel.ImageLoading;
using UnityEngine;
using UnityEngine.UI;

namespace MadoriVR.Scripts.CreateHouseModel.Core
{
    public sealed class ImageShower : MonoBehaviour, IImageShower
    {
        [SerializeField] private RawImage rawImage = default;
        [SerializeField] private Text message = default;

        private readonly UniTaskCompletionSource imageLoadedUtcs = new();
        public UniTask LoadImage() => imageLoadedUtcs.Task;

        private void Start()
        {
            rawImage.gameObject.SetActive(false);
        }

        public void ShowImage(Texture2D texture)
        {
            rawImage.texture = texture;
            var size = new Vector2(texture.width, texture.height);
            var scaleXY = size / 1000f;
            // FIX: サイズ調整を適切にする。この調整はPresenter側が指示を出すのが適切かも。
            scaleXY *= 3;
            rawImage.transform.localScale = new Vector3(scaleXY.x, scaleXY.y, 1.0f);
            
            rawImage.gameObject.SetActive(true);
            imageLoadedUtcs.TrySetResult();
        }

        public void ShowLoadResult(string text)
        {
            message.text = text;
        }
    }
}