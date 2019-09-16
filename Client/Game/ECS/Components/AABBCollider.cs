using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct AABBCollider : IComponentData
    {
        public AABB Box;
        public MinMaxAABB MinMaxBox;
        public int CollideCount;
    }
}