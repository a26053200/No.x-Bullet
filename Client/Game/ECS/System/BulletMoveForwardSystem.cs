using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class BulletMoveForwardSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//            Enabled = false;
        }

        [BurstCompile]
        private struct MoveForwardJob : IJobForEachWithEntity_ECCCC<Translation,MoveSpeed,Rotation,Bullet>
        {
            public float CurrentTime;
            public float DeltaTime;
            [ReadOnly]public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, ref Translation translation, ref MoveSpeed moveSpeed, ref Rotation rotation, ref Bullet bullet)
            {
                //Debug.Log(localToWorld.Forward);
                var dir = math.forward(rotation.Value);
                translation.Value.xyz += DeltaTime * moveSpeed.Speed * dir;
                if (CurrentTime - bullet.StartTime > 3f)
                {
                    EntityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new MoveForwardJob()
            {
                CurrentTime = Time.time,
                DeltaTime = Time.deltaTime,
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
//        protected override void OnCreateManager()
//        {
//            _buffer.Dispose();
//        }
    }
}