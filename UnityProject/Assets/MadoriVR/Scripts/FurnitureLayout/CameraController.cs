using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class CameraController : MonoBehaviour
    {
        private CameraMoveEventProvider eventProvider;
        private Camera mainCamera;

        private float initialFieldOfView;
        [SerializeField] private float minFieldOfView = 20;
        [SerializeField] private float maxFieldOfView = 100;
        [SerializeField] private float zoomChangeIntensity = 1.0f;

        [SerializeField] private float parallelMoveIntensity = 0.1f;
        
        
        private void Awake()
        {
            eventProvider = GetComponent<CameraMoveEventProvider>();
            
            mainCamera = GetComponent<Camera>();
            Assert.AreEqual(mainCamera, Camera.main);
            initialFieldOfView = mainCamera.fieldOfView;
        }

        private void Start()
        {
            eventProvider.OnZoomChange
                .Subscribe(value =>
                {
                    var newValue = mainCamera.fieldOfView + value * zoomChangeIntensity;
                    mainCamera.fieldOfView = Mathf.Clamp(newValue, minFieldOfView, maxFieldOfView);
                })
                .AddTo(this);

            eventProvider.OnCameraParallelDisplacement
                .Subscribe(value =>
                {
                    mainCamera.transform.localPosition += value * parallelMoveIntensity;
                })
                .AddTo(this);
        }
    }
}
