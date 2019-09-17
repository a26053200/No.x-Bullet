using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class AABBColliderSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        
        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _query = GetEntityQuery(ComponentType.ReadWrite<AABBCollider>(),
                ComponentType.ReadWrite<Bullet>(),
                ComponentType.ReadWrite<Translation>());
        }

        //[BurstCompile]
        private struct BulletColliderJob : IJobForEachWithEntity<Enemy, Airplane, AABBCollider>
        {
            [ReadOnly] public NativeArray<AABBCollider> Colliders;
            [ReadOnly] public NativeArray<Bullet> Bullets;
            [ReadOnly] public NativeArray<Translation> BulletTranslations;
            [ReadOnly] public NativeArray<Entity> Entities;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Enemy enemy,ref Airplane airplane, [ReadOnly] ref AABBCollider collider)
            {
                for (int i = index + 1; i < Colliders.Length; i++)
                {
                    var otherCollider = Colliders[i];
                    //击中
                    if (ECSPhysics.Intersect(collider.MinMaxBox, otherCollider.MinMaxBox))
                    {
                        collider.CollideCount += 1;
                        airplane.Hp = math.max(0f,airplane.Hp - Bullets[i].Damage);
                        EntityCommandBuffer.DestroyEntity(Entities[i]);
                        if (enemy.SpeedScale >= 1.0f)
                            enemy.SpeedScale *= 0.85f; //受到攻击后减速
                        //碰撞相交的新的AABB盒子,其中心点就是爆炸点
//                        var box = (AABB)ECSPhysics.GetEncompassingAABB(collider.MinMaxBox, otherCollider.MinMaxBox);
                        Entity e = EntityCommandBuffer.CreateEntity(ECSWorld.BlastEntityArchetype);
                        EntityCommandBuffer.SetComponent(e, new Blast
                        {
                            Pos = otherCollider.Box.Center,
                            Duration = Bullets[i].BlastDuration
                        });
                        break;
                    }
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var entities = _query.ToEntityArray(Allocator.TempJob);
            var colliders = _query.ToComponentDataArray<AABBCollider>(Allocator.TempJob);
            var bullets = _query.ToComponentDataArray<Bullet>(Allocator.TempJob);
            var translations = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var job = new BulletColliderJob()
            {
                Colliders = colliders,
                Bullets = bullets,
                BulletTranslations = translations,
                Entities = entities,        
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            colliders.Dispose();
            entities.Dispose();
            bullets.Dispose();
            translations.Dispose();
            return jobHandle;
        }
    }
}