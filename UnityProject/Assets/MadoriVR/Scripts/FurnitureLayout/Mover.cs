using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MadoriVR.Scripts.FurnitureLayout
{
    [RequireComponent(typeof(Collider))]
    public sealed class Mover : MonoBehaviour
    {
        private bool isSelect;
        
        private void Awake()
        {
            var col = GetComponent<Collider>();
        }

        public void Selected()
        {
            isSelect = true;
        }
        
        
    }
}