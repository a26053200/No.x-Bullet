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
    public class FiringSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreateManager()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            //Enabled = false;
        }
        
        private struct FiringJob : IJobForEachWithEntity_ECCCC<Airplane, Rotation, Firing, Translation>
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index,[ReadOnly] ref Airplane airplane, [ReadOnly] ref Rotation rotation, [ReadOnly] ref Firing firing,
                ref Translation translation)
            {
                if (!firing.IsFired)
                {
                    CreateBullet(airplane, FireStartTime, translation, EntityCommandBuffer, -airplane.BulletGap);
                    CreateBullet(airplane, FireStartTime, translation, EntityCommandBuffer, +airplane.BulletGap);
                    firing.IsFired = true;
                }
            }

            private void CreateBullet(Airplane airplane,float fireStartTime, Translation translation, EntityCommandBuffer buffer, float offsetX)
            {
                Entity entity = buffer.CreateEntity(ECSWorld.BulletEntityArchetype);
                float radian = math.PI / 180f;
                buffer.SetComponent(entity, new Rotation
                {
                    Value = quaternion.Euler(airplane.BulletEuler * radian, math.RotationOrder.Default)
                });
                buffer.SetComponent(entity, new Bullet
                {
                    StartTime = fireStartTime,
                    BoxSize = new float3(0.1f,0.1f,0.1f),
                    Damage = airplane.Damage,
                    BlastDuration = airplane.BulletBlastDuration
                });
                buffer.SetComponent(entity, new MoveSpeed
                {
                    Speed = airplane.BulletSpeed
                });
                buffer.SetComponent(entity, new NonUniformScale
                {
                    Value = airplane.BulletScale
                });
                var pos = translation.Value;
                buffer.SetComponent(entity, new Translation
                {
                    Value = new float3(pos.x + offsetX, pos.y, pos.z + airplane.ShootOffset),
                });
                buffer.SetSharedComponent(entity, new RenderMesh
                {
                    mesh = ECSWorld.Instance.meshBullet,
                    material = ECSWorld.Instance.materialBullet,
                    castShadows = ShadowCastingMode.Off,
                    receiveShadows = false
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