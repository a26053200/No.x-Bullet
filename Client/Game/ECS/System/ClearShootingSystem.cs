using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    public class ClearShootingSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;
        private EntityCommandBuffer _buffer;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//            Enabled = false;
        }
        //[BurstCompile]
        private struct ClearShootingJob : IJobForEachWithEntity<Airplane, Firing>
        {
            public float CurrentTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index,ref Airplane airplane, ref Firing firing)
            {
                if (CurrentTime - firing.FireStartTime > airplane.ShootIntervalTime)
                {
                    EntityCommandBuffer.RemoveComponent<Firing>(entity);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _buffer = _barrier.CreateCommandBuffer();
            var job = new ClearShootingJob()
            {
                CurrentTime = Time.time,
                EntityCommandBuffer = _buffer,
            };
            inputDeps = job.Schedule(this, inputDeps);
            _barrier.AddJobHandleForProducer(inputDeps);
            return inputDeps;
        }

        protected override void OnDestroyManager()
        {
            //_buffer.Dispose();
        }
    }
}