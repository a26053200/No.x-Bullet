using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
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
        
        private struct ClearEnemyJob : IJobForEachWithEntity<Enemy>
        {
            public float CurrentTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index,ref Enemy enemy)
            {
                if (CurrentTime - enemy.BornTime > enemy.LifeTime)
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
            inputDeps = job.Schedule(this, inputDeps);
            _barrier.AddJobHandleForProducer(inputDeps);
            return inputDeps;
        }
    }
}