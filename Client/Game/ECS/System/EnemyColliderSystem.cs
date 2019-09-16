using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Game
{
    public class EnemyColliderSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private EntityCommandBuffer _buffer;
        
        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _query = GetEntityQuery(ComponentType.ReadWrite<AABBCollider>(),
                ComponentType.ReadWrite<Enemy>());
        }
        
        private struct EnemyColliderJob : IJobForEachWithEntity<Player, AABBCollider>
        {
            [ReadOnly] public NativeArray<AABBCollider> Colliders;
            [ReadOnly] public NativeArray<Entity> Entities;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Player player, [ReadOnly] ref AABBCollider collider)
            {
                for (int i = index + 1; i < Colliders.Length; i++)
                {
                    var otherCollider = Colliders[i];
                    if (ECSPhysics.Intersect(collider.MinMaxBox, otherCollider.MinMaxBox))
                    {
                        collider.CollideCount += 1;
                        EntityCommandBuffer.DestroyEntity(Entities[i]);
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
            var job = new EnemyColliderJob()
            {
                Colliders = colliders,
                Entities = entities,        
                EntityCommandBuffer = _buffer,
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            colliders.Dispose();
            entities.Dispose();
            return jobHandle;
        }
    }
}