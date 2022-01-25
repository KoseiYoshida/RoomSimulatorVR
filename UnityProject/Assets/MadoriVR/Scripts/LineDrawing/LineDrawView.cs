using System.Collections.Generic;
using UnityEngine;

namespace MadoriVR.Scripts.LineDrawing
{
    /// <summary>
    /// Draw line for user.
    /// </summary>
    public sealed class LineDrawView : MonoBehaviour
    {
        [SerializeField] private Transform parent = default;
        
        private readonly Dictionary<int, LineRenderer> lines = new Dictionary<int, LineRenderer>(64);
        private int lineIndexCount;

        /// <summary>
        /// Draw line.
        /// </summary>
        /// <param name="points"></param>
        /// <returns>An unique index of a line</returns>
        public int DrawLine((Vector3, Vector3) points)
        {
            var go = new GameObject("Line");
            var line = go.AddComponent<LineRenderer>();
            go.transform.SetParent(parent);
            line.SetPosition(0, points.Item1);
            line.SetPosition(1, points.Item2);

            var index = lineIndexCount++;
            lines.Add(index, line);
            
            return index;
        }

        /// <summary>
        /// Move line position.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="points"></param>
        public void MoveLine(int lineIndex, (Vector3, Vector3) points)
        {
            var line = lines[lineIndex];
            line.SetPosition(0, points.Item1);
            line.SetPosition(1, points.Item2);
        }

        
        /// <summary>
        /// Delete line.
        /// </summary>
        /// <param name="lineIndex"></param>
        public void DeleteLine(int lineIndex)
        {
            var line = lines[lineIndex];
            lines.Remove(lineIndex);
            Destroy(line.gameObject);
        }
    }
}