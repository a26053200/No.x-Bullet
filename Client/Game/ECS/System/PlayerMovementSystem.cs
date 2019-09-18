using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class PlayerMovementSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
            Enabled = false;
        }

        [BurstCompile]
        private struct PlayerMovementJob : IJobForEachWithEntity_ECCCCCC<Player,Airplane,Translation,RotationEulerXYZ,MoveSpeed,PlayerInput>
        {
            public Rect Rect;
            public float DeltaTime;
            
            public void Execute(Entity entity, int index, [ReadOnly] ref Player player,[ReadOnly] ref Airplane airplane,ref Translation translation,
                ref RotationEulerXYZ rotationEulerXyz,
                ref MoveSpeed moveSpeed, ref PlayerInput input)
            {
                var pos = CalPos(input, translation.Value, moveSpeed.Speed, airplane.PlayerSize);
                translation.Value = pos;
                var radian = math.PI / 180f * 30f;
                rotationEulerXyz.Value = new float3(0, 0, -radian * input.Horizontal);
            }
            
            private float3 CalPos(PlayerInput input, float3 oldPos, float moveSpeed, float2 playerSize)
            {
                var pos = oldPos;
                pos += input.movement * DeltaTime * moveSpeed;
//                pos += new float3(
//                    input.Horizontal * DeltaTime * moveSpeed,
//                    0,
//                    input.Vertical * DeltaTime * moveSpeed);
                if (pos.x + playerSize.x > Rect.x + Rect.width)
                    pos.x = Rect.x + Rect.width - playerSize.x;
                else if (pos.x - playerSize.x < Rect.x)
                    pos.x = Rect.x + playerSize.x;
                if (pos.z + playerSize.y > Rect.y)
                    pos.z = Rect.y - playerSize.y;
                else if (pos.z - playerSize.y < Rect.y - Rect.height)
                    pos.z = Rect.y - Rect.height + playerSize.y;

                return pos;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ECSWorld.Instance)
            {
                var job = new PlayerMovementJob()
                {
                    Rect = ECSWorld.Instance.cornerRect,
                    DeltaTime = Time.deltaTime
                };
                return job.Schedule(this, inputDeps);
            }
            else
            {
                return inputDeps;
            }
        }
    }
}