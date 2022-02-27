using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MadoriVR.Scripts.CreateHouseModel.Core;
using MadoriVR.Scripts.ModelViewer.Data;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private WallLines wallLines = default;
    private void Awake()
    {
        // TODO: HouseModelGeneratorをCreateHouseModelと共通化する。
        var generator = new HouseModelGenerator
        {
            Height = 10f
        };
        var model = generator.Generate(wallLines.lines);
        // Scaleを適当に調整してる。
        model.transform.localScale *= 0.2f;
    }
    
    // TODO: 一時的な動作検証用。消す。
    class HouseModelGenerator
    {
        public float Height = 10;
        
        public GameObject Generate(IEnumerable<Line> lines)
        {
            var parent = new GameObject("Room");
            
            // Cubeを変形させ壁を表現する。X軸方向に長さを伸ばす。厚み(Z軸）は固定。Y軸周りに回転。
            foreach (var line in lines)
            {
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var wallTransform = wall.transform;
                wallTransform.SetParent(parent.transform);
                
                
                // ここの計算はベクトル演算をイメージすると理解しやすい。
                var start = line.point1;
                var end = line.point2;
                var length = (end - start).magnitude;
                var center2D = start + ((end - start) / 2);
                var angleDegree = CalculateDegree(start, end);
                
                // 3D上で表現するために変換
                const float PosY = 1.0f;
                var center3D = new Vector3(center2D.x, PosY, center2D.y);
                // y軸中心に回転させるのは決め打ち。角度はマイナスかける。
                var rotation = new Vector3(0,(float)-angleDegree, 0);
                
                wallTransform.SetPositionAndRotation(center3D, Quaternion.Euler(rotation));
                wallTransform.localScale = new Vector3(length, Height, 1);
            }
            
            // 床生成
            var minX = Mathf.Min(lines.Select(l => l.point1.x).Min(), lines.Select(l => l.point2.x).Min());
            var maxX = Mathf.Max(lines.Select(l => l.point1.x).Max(), lines.Select(l => l.point2.x).Max());
            var minZ = Mathf.Min(lines.Select(l => l.point1.y).Min(), lines.Select(l => l.point2.y).Min());
            var maxZ = Mathf.Min(lines.Select(l => l.point1.y).Max(), lines.Select(l => l.point2.y).Max());
            var center = new Vector2((maxX - minX) / 2, (maxZ - minZ) / 2);
            var width = maxX - minX;
            var depth = maxZ - minZ;
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            var planeTransform = plane.transform;
            planeTransform.SetParent(parent.transform);
            planeTransform.localPosition = new Vector3(center.x, -Height / 2,center.y);
            // planeはスケール1で10mなので、10で割っている。
            planeTransform.localScale = new Vector3(width / 10, 1f, depth / 10); 

            return parent;
        }
        
        private static double CalculateRadian(Vector2 point1, Vector2 point2)
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
        private static double CalculateDegree(Vector2 point1, Vector2 point2)
        {
            var degree = CalculateRadian(point1, point2) * 180d / Math.PI;
            return degree;
        }
    }
}
