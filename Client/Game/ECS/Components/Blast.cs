using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Blast : IComponentData
    {
        public float3 Pos;
        public float StartTime;
        public float Duration;
    }
}