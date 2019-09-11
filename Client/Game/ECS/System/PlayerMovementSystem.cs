using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerMovementSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Player player, ref Translation translation, ref RotationEulerXYZ rotationEulerXyz,
                ref MoveSpeed moveSpeed, ref PlayerInput input) =>
            {
                var pos = CalPos(input, translation.Value, moveSpeed.Speed);
                ;
                translation.Value = pos;
                var old = rotationEulerXyz.Value;
                var radian = math.PI / 180f * 30f;
                rotationEulerXyz.Value = new float3(0, 0, -radian * input.Horizontal);
            });

            Entities.ForEach((ref FireLight fireLight, ref MoveSpeed moveSpeed, ref PlayerInput input) =>
            {
                fireLight.pos = CalPos(input, fireLight.pos, moveSpeed.Speed);
            });
        }

        private float3 CalPos(PlayerInput input, float3 oldPos, float moveSpeed)
        {
            var pos = oldPos;
            pos += new float3(
                input.Horizontal * Time.deltaTime * moveSpeed,
                0,
                input.Vertical * Time.deltaTime * moveSpeed);
            Rect rect = ECSWorld.Instance.cornerRect;
            float2 playerSize = ECSWorld.Instance.playerSize;
            if (pos.x + playerSize.x > rect.x + rect.width)
                pos.x = rect.x + rect.width - playerSize.x;
            else if (pos.x - playerSize.x < rect.x)
                pos.x = rect.x + playerSize.x;
            if (pos.z + playerSize.y > rect.y)
                pos.z = rect.y - playerSize.y;
            else if (pos.z - playerSize.y < rect.y - rect.height)
                pos.z = rect.y - rect.height + playerSize.y;

            return pos;
        }
    }
}