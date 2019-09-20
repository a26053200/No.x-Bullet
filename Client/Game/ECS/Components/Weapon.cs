using Unity.Entities;
using UnityEngine;

namespace Game
{

    public struct Weapon : IComponentData
    {
        public float FireStartTime;
        
        public bool IsFired;

        public float Damage;

        public float ShootOffset;
        
        public float BulletSpeed;

        public float BulletBlastDuration;
        
        public Vector3 BulletScale;
        
        public Vector3 BulletEuler;

        public float BulletGap;
    }
}