using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class AirplaneInfo
    {
        public readonly Mesh Mesh;
        
        public readonly Material Material;
        
        public ShadowCastingMode ShadowCastingMode = ShadowCastingMode.Off;
        
        public bool ReceiveShadows = false;

        public readonly int Layer;
        
        public Vector3 BornPos = Vector3.zero;
        
        public float RotationY = 0f;

        public float MoveSpeed = 6f;
        
        public float ShootIntervalTime = 0.1f;

        public Vector3 Scale = Vector3.one;

        public Vector2 Size = new Vector2(1f, 2f);
        
        public Vector3 BoxSize = new Vector3(1f,1f,1f);

        public float ShootOffset = 0.5f;

        public float LifeTime = 10f;

        public float MaxHp = 100f;

        public float Damage = 20f;
        
        //Bullet
        public float BulletSpeed = 6f;

        public float BulletBlastDuration = 0.5f;
        
        public Vector3 BulletScale = Vector3.one;
        
        public Vector3 BulletEuler = new Vector3(90,180,0);
        
        public AirplaneInfo(Mesh mesh, Material material, int layer)
        {
            Mesh = mesh;
            Material = material;
            Layer = layer;
        }
    }
}