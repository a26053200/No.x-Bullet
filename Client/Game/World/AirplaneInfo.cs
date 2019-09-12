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

        public readonly int Layer;
        
        public Vector3 BornPos = Vector3.zero;
        
        public float RotationY = 0f;

        public float MoveSpeed = 6f;

        public float BulletSpeed = 6f;

        public float ShootIntervalTime = 0.1f;

        public float Scale = 1f;
        
        public float BulletScale = 1f;

        public Vector2 Size = new Vector2(1f, 2f);

        public float ShootOffset = 0.5f;

        public float LifeTime = 10f;
        
        public AirplaneInfo(Mesh mesh, Material material, int layer)
        {
            Mesh = mesh;
            Material = material;
            Layer = layer;
        }
    }
}