using UnityEngine.EventSystems;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public interface ISelectMessageHandler : IEventSystemHandler
    {
        void Select();

        void Unselect();
    }
}