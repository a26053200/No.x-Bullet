using UnityEngine;

namespace Game
{
    public class WeaponInfo
    {
        public int Level;

        public int No;
        
        public float Damage;

        public float ShootOffset;
        
        public float BulletSpeed = 6f;

        public float BulletBlastDuration = 0.5f;
        
        public Vector3 BulletScale = Vector3.one;
        
        public Vector3 BulletEuler = new Vector3(90,180,0);

        public Vector3 ShootDir = new Vector3(0,0,1);
        
        public float BulletGap;
    }
}