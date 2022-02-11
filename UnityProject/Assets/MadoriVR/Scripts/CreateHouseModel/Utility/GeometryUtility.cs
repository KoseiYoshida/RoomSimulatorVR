using System;
using UnityEngine;

namespace MadoriVR.Scripts.CreateHouseModel.Utility
{
    public static class GeometryUtility
    {
        /// <summary>
        /// Return angle between two points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>Angle between -PI~PI</returns>
        /// <remarks>Angle is from point1 to point2, start from the positive x-axis (where angle is 0).</remarks>
        public static double CalculateRadian(Vector2 point1, Vector2 point2)
        {
            // point1からみたときのpoint2の角度。x軸正方向を0°、反時計回りを正の角度とする。
            var rad = Math.Atan2(point2.y - point1.y, point2.x - point1.x);
            return rad;
        }
        
        /// <summary>
        /// Return angle between two points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>Angle between -179~180</returns>
        /// <remarks>Angle is from point1 to point2, start from the positive x-axis (where angle is 0).</remarks>
        public static double CalculateDegree(Vector2 point1, Vector2 point2)
        {
            var degree = CalculateRadian(point1, point2) * 180d / Math.PI;
            return degree;
        }
    }
}