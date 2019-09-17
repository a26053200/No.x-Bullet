using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class DebugSystem : ComponentSystem
    {
        protected override void OnCreateManager()
        {
            //Enabled = false;
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref AABBCollider aabb) =>
            {
                var c = aabb.Box.Center;
                var min = aabb.MinMaxBox.Min;
                var max = aabb.MinMaxBox.Max;
                var x = math.abs(max.x - min.x);
                var y = math.abs(max.y - min.y);
                var z = math.abs(max.z - min.z);

                var d1 = min;
                var d2 = new float3(min.x + x, min.y, min.z);
                var d3 = new float3(min.x + x, min.y, min.z + z);
                var d4 = new float3(min.x, min.y, min.z + z);
                var u1 = max;
                var u2 = new float3(max.x + x, max.y, max.z);
                var u3 = new float3(max.x + x, max.y, max.z + z);
                var u4 = new float3(max.x, max.y, max.z + z);
                Debug.DrawLine(d1, d2, Color.yellow);
                Debug.DrawLine(d2, d3, Color.yellow);
                Debug.DrawLine(d3, d4, Color.yellow);
                Debug.DrawLine(d4, d1, Color.yellow);
                
                Debug.DrawLine(u1, u2, Color.yellow);
                Debug.DrawLine(u2, u3, Color.yellow);
                Debug.DrawLine(u3, u4, Color.yellow);
                Debug.DrawLine(u4, u1, Color.yellow);
            });
        }
    }
}