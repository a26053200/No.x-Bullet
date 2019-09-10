using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace ECS
{
    public class PlayerMovementSystem : ComponentSystem
    {
        private float2 playerSize = new float2(1,1);
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Player player, ref Translation translation ,ref Rotation rotation, ref MoveSpeed moveSpeed, ref PlayerInput input) =>
            {
                var pos = translation.Value;
                pos += new float3(
                    input.Horizontal * Time.deltaTime * moveSpeed.Speed,
                    0,
                    input.Vertical * Time.deltaTime * moveSpeed.Speed);
                Rect rect = BootStrap.Instance.cornerRect;
                if (pos.x + playerSize.x > rect.x + rect.width)
                    pos.x = rect.x + rect.width - playerSize.x;
                else if (pos.x - playerSize.x < rect.x)
                    pos.x = rect.x + playerSize.x;
                if (pos.z + playerSize.y > rect.y + rect.height)
                    pos.z = rect.y + rect.height - playerSize.y;
                else if (pos.z - playerSize.y < rect.y)
                    pos.z = rect.y + playerSize.y;
                
                
                translation.Value = pos;
                var rot = input.Rotation;
                rotation.Value = rot;
            });
        }
    }
}