using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    public struct FireLight : IComponentData
    {
        public float StartTime;
        public float3 pos;
    }
}