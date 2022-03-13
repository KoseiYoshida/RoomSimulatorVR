using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class Selector : MonoBehaviour
    {
        private Camera mainCameraCache;

        private readonly ReactiveProperty<GameObject> selectedObject = new();
        public IReadOnlyReactiveProperty<GameObject> SelectedObject => selectedObject;

        private void Awake()
        {
            mainCameraCache = Camera.main;
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(value =>
                {
                    if (Physics.Raycast(mainCameraCache.ScreenPointToRay(Input.mousePosition), out var hit))
                    {
                        var handler = ExecuteEvents.ExecuteHierarchy<ISelectMessageHandler>(
                            root: hit.collider.gameObject,
                            eventData: null,
                            callbackFunction: (receiver, _) => receiver.Select()
                        );
                        
                        if(handler != null && handler != selectedObject.Value)
                        {
                            if (selectedObject.Value != null)
                            {
                                ExecuteEvents.Execute<ISelectMessageHandler>(
                                    target: selectedObject.Value,
                                    eventData: null,
                                    functor: (receiver, _) => receiver.Unselect()
                                );
                            }
                            
                            selectedObject.Value = handler;
                        }
                    }
                })
                .AddTo(this);
        }
        
    }
}