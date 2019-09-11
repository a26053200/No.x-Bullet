using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class AirplaneInfo
    {
        public readonly Mesh Mesh;
        
        public readonly Material Material;
        
        public readonly ShadowCastingMode ShadowCastingMode = ShadowCastingMode.Off;
        
        public readonly bool ReceiveShadows = false;

        public float Speed;

        public float BulletSpeed = 6f;

        public float ShootDeltaTime = 0.1f;

        public float BulletScale = 0.2f;

        public float2 PlayerSize = new float2(0.5f, 1);

        public float ShootOffset = 0.5f;
        
        public AirplaneInfo(Mesh mesh, Material material)
        {
            Mesh = mesh;
            Material = material;
        }
    }
}