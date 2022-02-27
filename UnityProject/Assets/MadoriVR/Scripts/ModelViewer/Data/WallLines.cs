using System.Collections.Generic;
using UnityEngine;

namespace MadoriVR.Scripts.ModelViewer.Data
{
    [CreateAssetMenu(fileName = "WallLines", menuName = "ScriptableObjects/WallLines")]
    public class WallLines : ScriptableObject
    {
        public IEnumerable<Line> lines;
    }
}