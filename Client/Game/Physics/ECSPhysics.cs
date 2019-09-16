using Unity.Mathematics;

namespace Game
{
    public static class ECSPhysics
    {
        public static bool Intersect(MinMaxAABB a, MinMaxAABB b)
        {
            return (a.Min.x <= b.Max.x && a.Max.x >= b.Min.x) &&
                   (a.Min.z <= b.Max.z && a.Max.z >= b.Min.z) &&
                   (a.Min.y <= b.Max.y && a.Max.y >= b.Min.y);
        }

        public static AABB GetEncompassingAABB(MinMaxAABB a, MinMaxAABB b)
        {
            MinMaxAABB returnAABB = new MinMaxAABB();

            returnAABB.Min = math.min(a.Min, b.Min);
            returnAABB.Max = math.max(a.Max, b.Max);

            return returnAABB;
        }

        public static void GrowAABB(ref MinMaxAABB sourceAABB, float3 includePoint)
        {
            sourceAABB.Min.x = math.min(sourceAABB.Min.x, includePoint.x);
            sourceAABB.Min.y = math.min(sourceAABB.Min.y, includePoint.y);
            sourceAABB.Min.z = math.min(sourceAABB.Min.z, includePoint.z);
            sourceAABB.Max.x = math.max(sourceAABB.Max.x, includePoint.x);
            sourceAABB.Max.y = math.max(sourceAABB.Max.y, includePoint.y);
            sourceAABB.Max.z = math.max(sourceAABB.Max.z, includePoint.z);
        }

        public static float3 GetAABBCenter(MinMaxAABB aabb)
        {
            return (aabb.Min + aabb.Max) * 0.5f;
        }
    }
}