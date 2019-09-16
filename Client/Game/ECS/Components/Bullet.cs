using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Bullet : IComponentData
    {
        public float Damage;
        public float StartTime;
        public float3 BoxSize;
    }
}