using System;

namespace MadoriVR.Scripts.ImageLoading
{
    public interface IImageSelector
    {
        public IObservable<string> OnPathEntered();
    }
}