using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{

    public struct Weapon : IComponentData
    {
        public int Level;

        public int No;
        
        public float FireStartTime;
        
        public bool IsFired;

        public float Damage;

        public float ShootOffset;
        
        public float BulletSpeed;

        public float BulletBlastDuration;
        
        public float3 BulletScale;
        
        public float3 BulletEuler;
        
        public float3 ShootDir;

        public float BulletGap;
    }
}