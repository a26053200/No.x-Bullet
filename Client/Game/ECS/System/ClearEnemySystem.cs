using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class ClearEnemySystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;
        private EntityCommandBuffer _buffer;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//            Enabled = false;
        }
        //[BurstCompile]
        private struct ClearEnemyJob : IJobForEachWithEntity<Enemy, Translation>
        {
            public float CurrentTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index,ref Enemy enemy,ref Translation translation)
            {
                if (enemy.BornTime > enemy.LifeTime && translation.Value.z < ECSWorld.Instance.cornerRect.y - ECSWorld.Instance.cornerRect.height)
                {
                    EntityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _buffer = _barrier.CreateCommandBuffer();
            var job = new ClearEnemyJob()
            {
                CurrentTime = Time.time,
                EntityCommandBuffer = _buffer,
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}