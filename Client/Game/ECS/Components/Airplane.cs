using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public struct Airplane : IComponentData
    {
        public float Hp;
        public float MaxHp;
        public float Damage;
        
        public float ShootIntervalTime;
        
        public float ShootOffset;
        
        public float BulletSpeed;

        public float BulletScale;

        public float2 PlayerSize;

        public float3 BoxSize;
    }
}