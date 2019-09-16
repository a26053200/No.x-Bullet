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
        
        [SerializeField]
        public Mesh meshBullet;
        
        [SerializeField]
        public Material materialBullet;
        
        public static EntityArchetype BulletEntityArchetype;

        public GameObjectEntity pointLightEntity;

        public Rect cornerRect;

        private CameraView _cameraView;

        private EntityManager _entityManager;

        private EntityArchetype _airplaneEntityArchetype;

        public void Launch()
        {
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
                typeof(Scale),
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
                typeof(CompositeRotation),
                typeof(Rotation),
                typeof(Scale),
                typeof(AABBCollider),
                typeof(Bullet)
            );

//            _entityManager.AddComponent<FireLight>(pointLightEntity.Entity);
//            _entityManager.AddComponent<MoveSpeed>(pointLightEntity.Entity);
//            _entityManager.SetComponentData(pointLightEntity.Entity, new MoveSpeed() {Speed = speed});
//            _entityManager.AddComponent<PlayerInput>(pointLightEntity.Entity);
        }

        public void CreateAirplane(AirplaneInfo info)
        {
            //实体的本地数组
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
            _entityManager.SetComponentData(entity, new Scale() {Value = info.Scale});
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
                    LifeTime = info.LifeTime
                });
            }
        }
    }
}