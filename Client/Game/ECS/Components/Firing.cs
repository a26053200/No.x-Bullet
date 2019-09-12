using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public struct Firing : IComponentData
    {
        public float FireStartTime;
        public bool IsFired;
    }
}