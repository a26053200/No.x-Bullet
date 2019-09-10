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

namespace ECS
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
//            Enabled = false;
        }


        //[BurstCompile]
        private struct PlayerShootingJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            public NativeArray<Entity> Entities;

            public void Execute(int index)
            {
                //if (FireStartTime > 0)
                {
                    EntityCommandBuffer.AddComponent(Entities[index], new Firing()
                    {
                        FireStartTime = FireStartTime
                    });
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (Input.GetButton("Fire1"))
            {
                _buffer = _barrier.CreateCommandBuffer();
                if(_weaponEntities != null && _weaponEntities.Length > 0)
                    _weaponEntities.Dispose();
                _weaponEntities = _query.ToEntityArray(Allocator.TempJob);
                var job = new PlayerShootingJob()
                {
                    Entities = _weaponEntities,
                    EntityCommandBuffer = _buffer,
                    FireStartTime = Time.time,
                };
                inputDeps = job.Schedule(_weaponEntities.Length, 1, inputDeps);
                _barrier.AddJobHandleForProducer(inputDeps);
                
            }

            return inputDeps;
        }

        protected override void OnCreateManager()
        {
            //_buffer.Dispose();
//            if(_weaponEntities.Length > 0)
//                _weaponEntities.Dispose();
        }
    }
}