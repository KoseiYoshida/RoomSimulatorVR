using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MadoriVR.Scripts.ModelViewer.Data
{
    [CreateAssetMenu(fileName = "DebugWallLines", menuName = "ScriptableObjects/DebugWallLines")]
    public sealed class DebugWallLines : WallLines, ISerializationCallbackReceiver
    {
        public List<Vector2Pair> pointSets = new();

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            lines = pointSets.Select(pair => new Line(pair.point1, pair.point2));
        }
        
        [System.Serializable]
        public class Vector2Pair
        {
            public Vector2 point1;
            public Vector2 point2;
        }
    }
}