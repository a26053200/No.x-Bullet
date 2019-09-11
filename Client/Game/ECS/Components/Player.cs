using Unity.Entities;

namespace Game
{
    public struct Player : IComponentData
    {
        public float Hp;
        public float Damage;
    }
}