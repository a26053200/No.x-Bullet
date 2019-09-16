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
        private struct AABBColliderMoveJob : IJobForEachWithEntity<AABBCollider, Translation, Airplane>
        {
            public void Execute(Entity entity, int index, ref AABBCollider aabb, ref Translation translation, ref Airplane airplane)
            {
                aabb.Box.Center = translation.Value;
                aabb.Box.Extents = airplane.BoxSize;
                aabb.MinMaxBox = aabb.Box;
            }
        }
        
        [BurstCompile]
        private struct AABBColliderBulletMoveJob : IJobForEachWithEntity<AABBCollider, Translation, Bullet>
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
            var job = new AABBColliderMoveJob();
            inputDeps = job.Schedule(this, inputDeps);
            var job1 = new AABBColliderBulletMoveJob();
            inputDeps = job1.Schedule(this, inputDeps);
            return inputDeps;
        }
    }
}