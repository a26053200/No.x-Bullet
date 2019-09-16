using Unity.Entities;

namespace Game
{
    public struct Enemy : IComponentData
    {
        public float BornTime;
        public float LifeTime;
    }
}