using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game
{
//    public class WeaponSystem : JobComponentSystem
//    {
//        [BurstCompile]
//        private struct AirplaneColliderJob : IJobForEachWithEntity<AABBCollider, Translation, Airplane>
//        {
//            public void Execute(Entity entity, int index, ref AABBCollider aabb, ref Translation translation, ref Airplane airplane)
//            {
//                aabb.Box.Center = translation.Value;
//                aabb.Box.Extents = airplane.BoxSize;
//                aabb.MinMaxBox = aabb.Box;
//            }
//        }
//        
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            var job = new AirplaneColliderJob();
//            var jobHandle = job.Schedule(this, inputDeps);
//            jobHandle.Complete();
//            return jobHandle;
//        }
//    }
}