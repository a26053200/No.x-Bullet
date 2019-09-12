using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Player : IComponentData
    {
        public float Hp;
        public float Damage;
    }
}