using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct FireLight : IComponentData
    {
        public float StartTime;
        public float3 pos;
    }
}