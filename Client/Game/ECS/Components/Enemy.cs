using Unity.Entities;

namespace Game
{
    public struct Enemy : IComponentData
    {
        public float Hp;
        public float Damage;
        public float BornTime;
        public float LifeTime;
    }
}