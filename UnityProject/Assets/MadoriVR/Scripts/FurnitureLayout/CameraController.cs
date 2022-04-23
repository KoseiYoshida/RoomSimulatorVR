using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace MadoriVR.Scripts.FurnitureLayout
{
    public sealed class CameraController : MonoBehaviour
    {
        private CameraMoveEventProvider eventProvider;
        private Camera mainCamera;
        private Transform mainCameraTransform;

        [SerializeField] private float minFieldOfView = 20;
        [SerializeField] private float maxFieldOfView = 100;
        [SerializeField] private float zoomChangeIntensity = 1.0f;

        [SerializeField] private float parallelMoveIntensity = 0.1f;

        [SerializeField] private float rotationIntensity = 1.0f;
        private Quaternion originRotation;
        
        
        private void Awake()
        {
            eventProvider = GetComponent<CameraMoveEventProvider>();
            
            mainCamera = GetComponent<Camera>();
            Assert.AreEqual(mainCamera, Camera.main);
            mainCameraTransform = mainCamera.transform;
        }

        private void Start()
        {
            eventProvider.OnZoomChanged
                .Subscribe(value =>
                {
                    var newValue = mainCamera.fieldOfView + value * zoomChangeIntensity;
                    mainCamera.fieldOfView = Mathf.Clamp(newValue, minFieldOfView, maxFieldOfView);
                })
                .AddTo(this);

            eventProvider.OnCameraParallelDisplacement
                .Subscribe(value =>
                {
                    mainCameraTransform.localPosition += value * parallelMoveIntensity;
                })
                .AddTo(this);

            eventProvider.OnCameraRotationDisplacement
                .Subscribe(value =>
                {
                    value *= rotationIntensity;
                    mainCameraTransform.localRotation *= Quaternion.Euler(-value.y, value.x, 0);
                })
                .AddTo(this);
        }
    }
}
