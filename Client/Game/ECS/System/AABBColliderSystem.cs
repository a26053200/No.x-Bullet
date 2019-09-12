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

        protected override void OnCreate()
        {
            _query = GetEntityQuery(ComponentType.ReadWrite<AABBCollider>());
        }

        [BurstCompile]
        private struct AABBColliderJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<AABBCollider> Colliders;

            public void Execute(int index)
            {
                for (int i = index + 1; i < Colliders.Length; i++)
                {
                    var selfCollider = Colliders[index];
                    var aabbCollider = Colliders[i];
                    if (selfCollider.Box.Contains(aabbCollider.Box))
                    {
                        //Debug.Log("AABBColliderJob");
                        selfCollider.CollideCount+= 1;
                        aabbCollider.CollideCount+= 1;
                    }
                    Colliders[index] = selfCollider;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var colliders = _query.ToComponentDataArray<AABBCollider>(Allocator.TempJob);
            var job = new AABBColliderJob()
            {
                Colliders = colliders,
            };
            var jobHandle = job.Schedule(colliders.Length, 64, inputDeps);
            jobHandle.Complete();
            colliders.Dispose();
            return jobHandle;
        }
    }
}