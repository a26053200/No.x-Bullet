using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public class BulletColliderMoveSystem: JobComponentSystem
    {
        [BurstCompile]
        private struct BulletColliderMoveJob : IJobForEachWithEntity<AABBCollider, Translation, Bullet>
        {
            public void Execute(Entity entity, int index, ref AABBCollider aabb, ref Translation translation, ref Bullet bullet)
            {
                aabb.Box.Center = translation.Value;
                aabb.Box.Extents = bullet.BoxSize;
                aabb.MinMaxBox = aabb.Box;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new BulletColliderMoveJob();
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}