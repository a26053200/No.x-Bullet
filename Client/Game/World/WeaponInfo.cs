using UnityEngine;

namespace Game
{
    public class WeaponInfo
    {
        public int SkinId;
        
        public int Level;

        public int No;
        
        public float Damage;

        public Vector3 ShootOffset;
        
        public float BulletSpeed = 6f;

        public float BulletBlastDuration = 0.5f;
        
        public Vector3 BulletScale = Vector3.one;
        
        public Vector3 BulletEuler = new Vector3(90,180,0);

        public Vector3 ShootDir = new Vector3(0,0,1);
        
        public Vector3 BoxSize = new Vector3(0.1f,0.1f,0.1f);
        
        public float BulletGap;
    }
}