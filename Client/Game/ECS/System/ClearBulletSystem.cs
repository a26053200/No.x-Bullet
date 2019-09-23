using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class ClearBulletSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;
        
        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            Enabled = false;
        }
        
        private struct ClearBulletJob : IJobForEachWithEntity<Bullet, Translation>
        {
            public float CurrentTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            public void Execute(Entity entity, int index, [ReadOnly] ref Bullet bullet, [ReadOnly] ref Translation translation)
            {
                if (bullet.IsHit)
                {
                    Entity e = EntityCommandBuffer.CreateEntity(ECSWorld.BlastEntityArchetype);
                    EntityCommandBuffer.SetComponent(e, new Blast
                    {
                        //Pos = translation.Value,
                        Duration = bullet.BlastDuration
                    });
                    EntityCommandBuffer.DestroyEntity(entity);
                }
                else
                {
                    if (CurrentTime - bullet.StartTime > ECSWorld.Instance.BulletLifeTime)
                    {
                        EntityCommandBuffer.DestroyEntity(entity);
                    }
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new ClearBulletJob()
            {
                CurrentTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}