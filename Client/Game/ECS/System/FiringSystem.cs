using ECS;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ECS
{
    public class FiringSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreateManager()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//            Enabled = false;
        }

        private struct FiringJob : IJobForEachWithEntity_ECCC<Rotation, Firing, Translation>
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, ref Rotation rotation, ref Firing firing,
                ref Translation translation)
            {
                if (!firing.IsFired)
                {
                    CreateBullet(FireStartTime, translation, rotation, EntityCommandBuffer);
                    firing.IsFired = true;
                }
            }

            private void CreateBullet(float fireStartTime, Translation translation, Rotation rotation,
                EntityCommandBuffer buffer)
            {
                //Debug.Log("Generate a bullet");
                Entity entity = buffer.CreateEntity(BootStrap.BulletEntityArchetype);
                buffer.SetComponent(entity, rotation);
                buffer.SetComponent(entity, new Bullet
                {
                    StartTime = fireStartTime
                });
                buffer.SetComponent(entity, new MoveSpeed
                {
                    Speed = BootStrap.Instance.bulletSpeed
                });
                buffer.SetComponent(entity, new Scale
                {
                    Value = BootStrap.Instance.bulletScale
                });
                var pos = translation.Value;
                
                buffer.SetComponent(entity, new Translation
                {
                    Value = new float3(pos.x, pos.y, pos.z + BootStrap.Instance.shootOffset),
                });
                buffer.SetSharedComponent(entity, new RenderMesh
                {
                    mesh = BootStrap.Instance.mesh,
                    material = BootStrap.Instance.material,
                    castShadows = ShadowCastingMode.On,
                    receiveShadows = true
                });
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new FiringJob()
            {
                FireStartTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer()
            };
            return job.Schedule(this, inputDeps);
        }
    }
}