using System;
using MadoriVR.Scripts.LineDrawing;
using NaughtyAttributes;
using UnityEngine;
using VContainer.Unity;

namespace MadoriVR
{
    public sealed class TestMono : MonoBehaviour
    {
        [SerializeField] private LifetimeScope lineDrawLifetimeScope = default;
        private DrawnLineModel _drawnLineModel;
    
        private void Start()
        {
            _drawnLineModel = (DrawnLineModel)lineDrawLifetimeScope.Container.Resolve(typeof(DrawnLineModel));
        }

        [Button("Check")]
        private void Check()
        {
            foreach (var line in _drawnLineModel.Lines)
            {
                Debug.Log($"{line.point1} - {line.point2}");
            }
        }
    }
}