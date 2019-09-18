using System.Numerics;
using Unity.Entities;
using Unity.Mathematics;
using Quaternion = UnityEngine.Quaternion;

namespace Game
{
    public struct PlayerInput : IComponentData
    {
        public float Horizontal;
        public float Vertical;
        public Quaternion Rotation;
        public float3 movement;
    }
}