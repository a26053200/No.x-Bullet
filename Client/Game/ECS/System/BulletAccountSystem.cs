using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class BulletAccountSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref Bullet bullet, ref Translation translation) =>
            {
                if (bullet.IsHit)
                {
                    Entity e = PostUpdateCommands.CreateEntity(ECSWorld.BlastEntityArchetype);
                    PostUpdateCommands.SetComponent(e, new Blast
                    {
                        //Pos = translation.Value,
                        Duration = bullet.BlastDuration
                    });
                    PostUpdateCommands.DestroyEntity(entity);
                }
                else
                {
                    if (Time.time - bullet.StartTime > ECSWorld.Instance.BulletLifeTime)
                    {
                        PostUpdateCommands.DestroyEntity(entity);
                    }
                }
            });
        }
    }
}