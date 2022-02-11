using System.Collections.Generic;
using MadoriVR.Scripts.CreateHouseModel.LineDrawing;
using UnityEngine;

namespace MadoriVR.Scripts.CreateHouseModel.Core
{
    public sealed class HouseModelGenerator
    {
        public GameObject Generate(IEnumerable<ImmutableLine> lines)
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
                var angleDegree = Utility.GeometryUtility.CalculateDegree(start, end);
                
                // 3D上で表現するために変換
                const float PosY = 1.0f;
                var center3D = new Vector3(center2D.x, PosY, center2D.y);
                // y軸中心に回転させるのは決め打ち。角度はマイナスかける。
                var rotation = new Vector3(0,(float)-angleDegree, 0);
                
                wallTransform.SetPositionAndRotation(center3D, Quaternion.Euler(rotation));
                const float Height = 3;
                wallTransform.localScale = new Vector3(length, Height, 1);
            }

            return parent;
        }
    }
}