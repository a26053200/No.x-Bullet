using System;
using Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace ECS
{
    public class BootStrap : MonoBehaviour
    {
        public static BootStrap Instance;

        public static EntityArchetype FireEntityArchetype;

        public static EntityArchetype BulletEntityArchetype;

        [SerializeField] public Mesh meshDragon;

        [SerializeField] public Material materialDragon;

//        [SerializeField]
//        private GameObjectEntity gameObjectEntity;
//
//        [SerializeField]
//        private GameObjectEntity weaponEntity;

        [SerializeField] public Mesh mesh;

        [SerializeField] public Material material;

        [SerializeField] private float speed;

        [SerializeField] public float bulletSpeed = 6f;

        [SerializeField] public float shootDeltaTime = 0.1f;

        [SerializeField] public float bulletScale = 0.2f;

        [SerializeField] public float2 playerSize = new float2(0.5f, 1);

        [SerializeField] public float shootOffset = 0.5f;

        public GameObjectEntity pointLightEntity;

        public Rect cornerRect;

        private CameraView _cameraView;

        public void Launch()
        {
            if (Camera.main != null) _cameraView = Camera.main.GetComponent<CameraView>();
            Instance = this;
            EntityManager entityManager = World.Active.EntityManager;

            EntityArchetype airplaneEntityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(PlayerInput),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Scale),
                typeof(CompositeRotation),
                typeof(Rotation),
                typeof(RotationEulerXYZ),
                typeof(Player),
                typeof(Weapon)
            );
            EntityArchetype weaponEntityArchetype = entityManager.CreateArchetype(
                typeof(Weapon)
//                typeof(PlayerInput),
//                typeof(LocalToWorld),
//                typeof(Translation),
//                typeof(Rotation)
            );
            FireEntityArchetype = entityManager.CreateArchetype(typeof(Firing));

            BulletEntityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Rotation),
                typeof(Scale),
                typeof(Bullet)
            );

            //实体的本地数组
            Entity entity = entityManager.CreateEntity(airplaneEntityArchetype);
            entityManager.SetComponentData(entity, new MoveSpeed() {Speed = speed});
            entityManager.SetComponentData(entity, new Scale() {Value = 0.5f});
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = meshDragon,
                material = materialDragon,
                castShadows = ShadowCastingMode.On,
                receiveShadows = true
            });

            entityManager.AddComponent<FireLight>(pointLightEntity.Entity);
            entityManager.AddComponent<MoveSpeed>(pointLightEntity.Entity);
            entityManager.SetComponentData(pointLightEntity.Entity, new MoveSpeed() {Speed = speed});
            entityManager.AddComponent<PlayerInput>(pointLightEntity.Entity);
//            Entity weaponEntity = entityManager.CreateEntity(weaponEntityArchetype);
            //CreateBullet();
        }

        private void Update()
        {
            if (_cameraView)
                cornerRect = _cameraView.rect;
        }
    }
}