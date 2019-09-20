using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerShootingSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private NativeArray<Entity> _weaponEntities;
        private EntityCommandBuffer _buffer;
        
        protected override void OnCreate()
        {
            _query = GetEntityQuery(
                ComponentType.ReadWrite<Weapon>(),
                ComponentType.Exclude<Firing>());
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            Enabled = false;
        }


        //[BurstCompile] 
        private struct PlayerShootingJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly] public NativeArray<Entity> Entities;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(int index)
            {
                EntityCommandBuffer.AddComponent(Entities[index], new Firing()
                {
                    FireStartTime = FireStartTime
                });
            }
        }

        private struct FileLightJob : IJobForEachWithEntity<FireLight>
        {
            public float CurrTime;
            public void Execute(Entity entity, int index, ref FireLight fireLight)
            {
                fireLight.StartTime = CurrTime;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ECSWorld.Instance.isStart)
            {
                _weaponEntities = _query.ToEntityArray(Allocator.TempJob);
                var job = new PlayerShootingJob()
                {
                    Entities = _weaponEntities,
                    EntityCommandBuffer = _barrier.CreateCommandBuffer(),
                    FireStartTime = Time.time,
                };
                var jobHandle = job.Schedule(_weaponEntities.Length, 64, inputDeps);
                jobHandle.Complete();
                _weaponEntities.Dispose();
                return jobHandle;
            }else
                return inputDeps;
        }
    }
}