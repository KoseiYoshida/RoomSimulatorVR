using UnityEngine;

namespace MadoriVR.Scripts.ImageLoading
{
    public interface IImageShower
    {
        public void ShowImage(Texture2D texture);

        public void ShowLoadResult(string text);
    }
}