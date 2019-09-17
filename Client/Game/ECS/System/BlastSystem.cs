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
    public class BlastSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;
        
        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        private struct BlastCreateJob : IJobForEachWithEntity<Blast>
        {
            public float CurrentTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            
            public void Execute(Entity entity, int index, ref Blast blast)
            {
                if (blast.StartTime <= 0)
                {
                    blast.StartTime = CurrentTime;
                    CreateBlast(entity, blast, EntityCommandBuffer);
                }else if (CurrentTime - blast.StartTime > blast.Duration)
                {
                    EntityCommandBuffer.DestroyEntity(entity);
                }
            }
            
            private void CreateBlast(Entity entity, Blast blast, EntityCommandBuffer buffer)
            {
                float radian = math.PI / 180f;
                buffer.SetComponent(entity, new Rotation
                {
                    Value = quaternion.Euler(90 * radian,0,0, math.RotationOrder.Default),
                });
                buffer.SetComponent(entity, new NonUniformScale
                {
                    Value = new float3(1,1,1)
                });
                buffer.SetComponent(entity, new Translation
                {
                    Value = blast.Pos,
                });
                buffer.SetSharedComponent(entity, new RenderMesh
                {
                    mesh = ECSWorld.Instance.meshBlast,
                    material = ECSWorld.Instance.materialBlast,
                    castShadows = ShadowCastingMode.Off,
                    receiveShadows = false
                });
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new BlastCreateJob()
            {
                CurrentTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}