using Unity.Entities;

namespace Game
{
    public class EnemyAccountSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref Enemy enemy, ref Airplane airplane) =>
            {
                if (airplane.Hp <= 0)
                {
                    PostUpdateCommands.DestroyEntity(entity);
                    ECSWorld.Instance.score++;
                    PostUpdateCommands.CreateEntity(ECSWorld.BlastEntityArchetype);
                }
            });
        }
    }
}