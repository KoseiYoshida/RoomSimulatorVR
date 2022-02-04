using System;

namespace MadoriVR.Scripts.CreateHouseModel.ImageLoading
{
    public interface IImageSelector
    {
        public IObservable<string> OnPathEntered();
    }
}