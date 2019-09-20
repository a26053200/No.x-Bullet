﻿using Unity.Entities;
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

        public float BulletBlastDuration;
        
        public float3 BulletEuler;

        public float3 BulletScale;

        public float BulletGap;

        public float2 PlayerSize;

        public float3 BoxSize;
    }
}