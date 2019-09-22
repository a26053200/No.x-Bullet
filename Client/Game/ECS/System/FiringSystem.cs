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
        
        private struct FiringJob : IJobForEachWithEntity_ECC<Weapon, Translation>
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Weapon weapon,ref Translation translation)
            {
                if (!weapon.IsFired)
                {
                    weapon.FireStartTime = FireStartTime;
                    if ((weapon.No == 0 || weapon.No == ECSWorld.Instance.ActiveWeaponNo) && weapon.Level <= ECSWorld.Instance.ActiveWeaponLevel)
                    {
                        CreateBullet(weapon, FireStartTime, translation, EntityCommandBuffer, weapon.BulletGap);
                        weapon.IsFired = true;
                    }
                }
            }

            private void CreateBullet(Weapon weapon,float fireStartTime, Translation translation, EntityCommandBuffer buffer, float offsetX)
            {
                Entity entity = buffer.CreateEntity(ECSWorld.BulletEntityArchetype);
                float radian = math.PI / 180f;
                buffer.SetComponent(entity, new Rotation
                {
                    Value = quaternion.Euler(weapon.BulletEuler * radian, math.RotationOrder.Default)
                });
                buffer.SetComponent(entity, new Bullet
                {
                    StartTime = fireStartTime,
                    BoxSize = new float3(0.1f * weapon.BulletScale.x,0.1f,0.1f),
                    Damage = weapon.Damage,
                    BlastDuration = weapon.BulletBlastDuration,
                    MoveDir = weapon.ShootDir
                });
                buffer.SetComponent(entity, new MoveSpeed
                {
                    Speed = ECSWorld.Instance.BulletSpeed
                });
                buffer.SetComponent(entity, new NonUniformScale
                {
                    Value = weapon.BulletScale
                });
                var pos = translation.Value + weapon.ShootOffset;
                buffer.SetComponent(entity, new Translation
                {
                    Value = new float3(pos.x + offsetX, pos.y, pos.z ),
                });
                buffer.SetSharedComponent(entity, new RenderMesh
                {
                    mesh = ECSWorld.Instance.meshBullets[weapon.SkinId],
                    material = ECSWorld.Instance.materialBullets[weapon.SkinId],
                    castShadows = ShadowCastingMode.Off,
                    receiveShadows = false
                });
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ECSWorld.Instance && ECSWorld.Instance.isStart)
            {
                var job = new FiringJob()
                {
                    FireStartTime = Time.time,
                    EntityCommandBuffer = _barrier.CreateCommandBuffer()
                };
                var jobHandle = job.Schedule(this, inputDeps);
                jobHandle.Complete();
                return jobHandle;
            }else
                return inputDeps;
        }
    }
}