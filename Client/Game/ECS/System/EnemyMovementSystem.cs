using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class EnemyMovementSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct EnemyMovementJob : IJobForEachWithEntity_ECCCCCC<Enemy,Airplane,Translation,RotationEulerXYZ,MoveSpeed,PlayerInput>
        {
            public Rect Rect;
            public float DeltaTime;
            
            public void Execute(Entity entity, int index, ref Enemy enemy, ref Airplane airplane, ref Translation translation,
                ref RotationEulerXYZ rotationEulerXyz,
                ref MoveSpeed moveSpeed, ref PlayerInput input)
            {
                var pos = translation.Value;
                pos += new float3(0, 0, -DeltaTime * moveSpeed.Speed);
                translation.Value = pos;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ECSWorld.Instance)
            {
                var job = new EnemyMovementJob()
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