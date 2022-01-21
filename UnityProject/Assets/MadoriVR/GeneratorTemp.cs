using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MadoriVR
{
    public sealed class GeneratorTemp : MonoBehaviour
    {
        private readonly List<(Vector2 p1, Vector2 p2)> lines = new List<(Vector2, Vector2)>();
        

        private void Start()
        {
            var clickPointProvider = GetComponent<ClickPointProvider>();
            clickPointProvider.OnClicked
                .Buffer(2)
                .Subscribe(poitns =>
                {
                    var p1 = poitns[0];
                    var p2 = poitns[1];
                    lines.Add((p1, p2));
                    Debug.Log($"add:{p1}-{p2}");
                }).AddTo(this);
        }


        [ContextMenu("Generate")]
        private void GenerateMadori()
        {
            var parent = new GameObject("Room");
            
            foreach (var line in lines)
            {
                var start = line.p1;
                var end = line.p2;

                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var wallTransform = wall.transform;
                wallTransform.SetParent(parent.transform);

                var width = Mathf.Abs(end.x - start.x);
                var depth = Mathf.Abs(end.y - start.y);
                
                // 細くなり過ぎるのを防ぐ。
                wallTransform.localScale = new Vector3(
                    Mathf.Max(width, 10), 
                    1, 
                    Mathf.Max(depth, 10)
                );

                const float HEIGHT = 1.0f;
                
                wallTransform.localPosition = new Vector3(
                    Mathf.Min(start.x, end.x) + width / 2, 
                    HEIGHT, 
                    Mathf.Min(start.y, end.y) + depth / 2
                );
                
                Debug.Log($"s:{start}, e:{end}, pos:{wallTransform.localPosition}");
            }

        }

        // [ContextMenu("Dummy data")]
        // public void DummyData()
        // {
        //     var origin = new Vector2(100f, 30f);
        //     var diffX = new Vector2(100f, 0f);
        //     var diffY = new Vector2(0f, 150f);
        //     
        //     madori = new Madori();
        //     madori.AddLine(origin, origin + diffX);
        //     madori.AddLine(origin, origin + diffY);
        //     madori.AddLine(origin + diffX, origin + diffX + diffY);
        //     madori.AddLine(origin + diffY, origin + diffX + diffY);
        //     
        //     madori.Dump();
        // }
        //
        // [ContextMenu("Show Madori Data")]
        // public void ShowMadoriData()
        // {
        //     madori.Dump();
        // }
    }
}