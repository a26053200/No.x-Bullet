using Unity.Entities;

namespace ECS
{
    public struct Firing : IComponentData
    {
        public float FireStartTime;
        public bool IsFired;
    }
}