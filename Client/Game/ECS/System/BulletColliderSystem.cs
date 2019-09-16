using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class AABBColliderSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private EntityCommandBuffer _buffer;
        
        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _query = GetEntityQuery(ComponentType.ReadWrite<AABBCollider>(),
                ComponentType.ReadWrite<Bullet>());
        }

        [BurstCompile]
        private struct BulletColliderJob : IJobForEachWithEntity<Enemy, Airplane, AABBCollider>
        {
            [ReadOnly] public NativeArray<AABBCollider> Colliders;
            [ReadOnly] public NativeArray<Bullet> Bullets;
            [ReadOnly] public NativeArray<Entity> Entities;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Enemy enemy,ref Airplane airplane, [ReadOnly] ref AABBCollider collider)
            {
                for (int i = index + 1; i < Colliders.Length; i++)
                {
                    var otherCollider = Colliders[i];
                    if (ECSPhysics.Intersect(collider.MinMaxBox, otherCollider.MinMaxBox))
                    {
                        collider.CollideCount += 1;
                        airplane.Hp = math.max(0f,airplane.Hp - Bullets[i].Damage);
                        EntityCommandBuffer.DestroyEntity(Entities[i]);
                        if(airplane.Hp <= 0)
                            EntityCommandBuffer.DestroyEntity(entity);
                        break;
                    }
                }
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _buffer = _barrier.CreateCommandBuffer();
            var entities = _query.ToEntityArray(Allocator.TempJob);
            var colliders = _query.ToComponentDataArray<AABBCollider>(Allocator.TempJob);
            var bullets = _query.ToComponentDataArray<Bullet>(Allocator.TempJob);
            var job = new BulletColliderJob()
            {
                Colliders = colliders,
                Bullets = bullets,
                Entities = entities,        
                EntityCommandBuffer = _buffer,
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            colliders.Dispose();
            entities.Dispose();
            bullets.Dispose();
            return jobHandle;
        }
    }
}