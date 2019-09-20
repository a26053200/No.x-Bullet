using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game
{
    public class WeaponMoveSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        
        protected override void OnCreate()
        {
            _query = GetEntityQuery(ComponentType.ReadWrite<Weapon>());
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            //Enabled = false;
        }
        
        //[BurstCompile]
        private struct WeaponMoveJob : IJobForEachWithEntity<Player, Translation>
        {
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            [ReadOnly] public NativeArray<Entity> Entities;
            
            public void Execute(Entity entity, int index, [ReadOnly] ref Player player, [ReadOnly] ref Translation translation)
            {
                for (int i = 0; i < Entities.Length; i++)
                {
                    EntityCommandBuffer.SetComponent(Entities[i], translation);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var entities = _query.ToEntityArray(Allocator.TempJob);
            var job = new WeaponMoveJob()
            {
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
                Entities = entities
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            entities.Dispose();
            return jobHandle;
        }
    }
}