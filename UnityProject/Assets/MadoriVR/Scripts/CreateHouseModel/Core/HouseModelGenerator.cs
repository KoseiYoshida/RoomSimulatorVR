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
            
            foreach (var line in lines)
            {
                var start = line.point1;
                var end = line.point2;

                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var wallTransform = wall.transform;
                wallTransform.SetParent(parent.transform);

                var width = Mathf.Abs(end.x - start.x);
                var depth = Mathf.Abs(end.y - start.y);

                // 壁に適度な厚みをもたせる
                const float MIN_THICKNESS = 3;
                const float HEIGHT = 3;
                wallTransform.localScale = new Vector3(
                    Mathf.Max(width, MIN_THICKNESS), 
                    HEIGHT, 
                    Mathf.Max(depth, MIN_THICKNESS)
                );

                const float POS_Y = 1.0f;
                wallTransform.localPosition = new Vector3(
                    Mathf.Min(start.x, end.x) + width / 2, 
                    POS_Y, 
                    Mathf.Min(start.y, end.y) + depth / 2
                );
            }

            return parent;
        }
    }
}