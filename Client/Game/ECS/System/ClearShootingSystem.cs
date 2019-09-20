using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    public class ClearShootingSystem : JobComponentSystem
    {

        protected override void OnCreate()
        {
//            Enabled = false;
        }
        [BurstCompile]
        private struct ClearShootingJob : IJobForEachWithEntity<Weapon>
        {
            public float ShootIntervalTime;
            public float CurrentTime;
            public void Execute(Entity entity, int index,ref Weapon weapon)
            {
                if (weapon.FireStartTime > 0 && CurrentTime - weapon.FireStartTime > ShootIntervalTime)
                {
                    weapon.FireStartTime = 0;
                    weapon.IsFired = false;
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!ECSWorld.Instance)
                return inputDeps;
            var shootIntervalTime = 1f / ECSWorld.Instance.ShootSpeed;
            //Debug.Log("shootIntervalTime " + shootIntervalTime);
            var job = new ClearShootingJob()
            {
                ShootIntervalTime = shootIntervalTime,
                CurrentTime = Time.time,
            };
            var jobHandle = job.Schedule(this, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}