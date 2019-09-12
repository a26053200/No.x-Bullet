using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public class AABBColliderMoveSystem: JobComponentSystem
    {
        [BurstCompile]
        private struct AABBColliderMoveJob : IJobForEachWithEntity<AABBCollider, Translation>
        {
            public void Execute(Entity entity, int index, ref AABBCollider aabb, ref Translation translation)
            {
                aabb.Box.Center = translation.Value;
                aabb.Box.Extents = new float3(1,1,1);
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new AABBColliderMoveJob();
            return job.Schedule(this, inputDeps);
        }
    }
}