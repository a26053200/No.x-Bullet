using System;
using Framework;
using LuaInterface;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Game
{
    public class ECSWorld : MonoBehaviour
    {
        public static ECSWorld Instance;

        [HideInInspector]
        public int score;

        [Range(1f, 60f)] public float ShootSpeed = 0;
        
        [Range(1f, 100f)] public float BulletSpeed = 0;
        
        [Range(0.1f, 5f)] public float BulletLifeTime = 3f;
        public bool isStart { get; private set; }

        public float Horizontal;
        
        public float Vertical;
        
        public int ActiveWeaponNo;
        
        public int ActiveWeaponLevel;
        
        [SerializeField]
        public Mesh[] meshBullets;
        
        [SerializeField]
        public Material[] materialBullets;
        
        [SerializeField]
        public Mesh meshBlast;
        
        [SerializeField]
        public Material materialBlast;
        
        public static EntityArchetype BulletEntityArchetype;

        public static EntityArchetype BlastEntityArchetype;
        
        public GameObjectEntity pointLightEntity;

        public Rect cornerRect;

        private CameraView _cameraView;

        private EntityManager _entityManager;

        private EntityArchetype _airplaneEntityArchetype;

        private EntityArchetype _weaponEntityArchetype;

        public void Launch()
        {
            if (Instance)
                throw new Exception("This class can only create one instance");
            if (Camera.main != null)
                _cameraView = Camera.main.GetComponent<CameraView>();
            if (_cameraView)
                cornerRect = _cameraView.rect;
            Instance = this;
            
            _entityManager = Unity.Entities.World.Active.EntityManager;
            _airplaneEntityArchetype = _entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(PlayerInput),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(NonUniformScale),
                typeof(CompositeRotation),
                typeof(Rotation),
                typeof(RotationEulerXYZ),
                typeof(AABBCollider),
                typeof(Airplane)
            );
            BulletEntityArchetype = _entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                //typeof(LocalToParent),
                //typeof(ParentScaleInverse),
                //typeof(CompositeScale),
                //typeof(CompositeRotation),
                typeof(Rotation),
                typeof(NonUniformScale),
                typeof(AABBCollider),
                typeof(Bullet)
            );
            
            BlastEntityArchetype = _entityManager.CreateArchetype(
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(NonUniformScale),
                typeof(Rotation),
                typeof(Blast)
            );

            _weaponEntityArchetype = _entityManager.CreateArchetype(
                typeof(Translation),
                typeof(Weapon)
            );
        }

        public void StartGame()
        {
            isStart = true;
        }
        
        
        public void CreateAirplane(AirplaneInfo info)
        {
            Entity entity = _entityManager.CreateEntity(_airplaneEntityArchetype);
            _entityManager.SetComponentData(entity, new MoveSpeed() {Speed = info.MoveSpeed});
            _entityManager.SetComponentData(entity, new Translation()
            {
                Value = info.BornPos
            });
            var radian = math.PI / 180f * info.RotationY;
            _entityManager.SetComponentData(entity, new RotationEulerXYZ()
            {
                Value = new float3(0, radian, 0)
            });
            _entityManager.SetComponentData(entity, new NonUniformScale() {Value = info.Scale});
            _entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = info.Mesh,
                material = info.Material,
                castShadows = info.ShadowCastingMode,
                receiveShadows = info.ReceiveShadows,
                layer = info.Layer,
            });
            _entityManager.SetComponentData(entity, new Airplane()
            {
                //MeshBullet = info.MeshBullet,
                //MaterialBullet = info.MaterialBullet,
                BulletScale = info.BulletScale,
                BulletSpeed = info.BulletSpeed,
                BulletEuler = info.BulletEuler,
                BulletGap = info.BulletGap,
                BulletBlastDuration = info.BulletBlastDuration,
                ShootIntervalTime = info.ShootIntervalTime,
                ShootOffset = info.ShootOffset,
                PlayerSize = info.Size,
                BoxSize = info.BoxSize,
                Hp = info.MaxHp,
                MaxHp = info.MaxHp,
                Damage = info.Damage
            });
            if (info.Layer == LayerMask.NameToLayer("Hero"))
            {
                _entityManager.AddComponent<Player>(entity);
            }
            else
            {
                _entityManager.AddComponent<Enemy>(entity);
                _entityManager.AddComponent<AABBCollider>(entity);
                _entityManager.SetComponentData(entity, new Enemy
                {
                    BornTime = Time.time,
                    LifeTime = info.LifeTime,
                    SpeedScale = 1
                });
            }
        }

        public Weapon CreateWeapon(WeaponInfo info)
        {
            Entity entity = _entityManager.CreateEntity(_weaponEntityArchetype);
            var weapon = new Weapon
            {
                SkinId = info.SkinId,
                No = info.No,
                Level = info.Level,
                Damage = info.Damage,
                ShootOffset = info.ShootOffset,
                BulletScale = info.BulletScale,
                BulletSpeed = info.BulletSpeed,
                BulletEuler = info.BulletEuler,
                BulletGap = info.BulletGap,
                BoxSize = info.BoxSize,
                BulletBlastDuration = info.BulletBlastDuration,
                ShootDir = info.ShootDir,
            };
            _entityManager.SetComponentData(entity, weapon);
            return weapon;
        }
    }
}