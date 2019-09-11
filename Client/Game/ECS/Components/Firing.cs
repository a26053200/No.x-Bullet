using Unity.Entities;

namespace Game
{
    public struct Firing : IComponentData
    {
        public float FireStartTime;
        public bool IsFired;
    }
}