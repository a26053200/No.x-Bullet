using System;
using Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace ECS
{
    public class BootStrap : MonoBehaviour
    {
        public static BootStrap Instance;

        public static EntityArchetype FireEntityArchetype;
        
        public static EntityArchetype BulletEntityArchetype;
        
        [SerializeField]
        public Mesh meshDragon;

        [SerializeField]
        public Material materialDragon;
        
//        [SerializeField]
//        private GameObjectEntity gameObjectEntity;
//
//        [SerializeField]
//        private GameObjectEntity weaponEntity;
        
        [SerializeField]
        public Mesh mesh;

        [SerializeField]
        public Material material;
        
        [SerializeField]
        private float speed;
        
        [SerializeField]
        public float bulletSpeed = 6f;
        
        [SerializeField]
        public float shootDeltaTime = 0.1f;
        
        [SerializeField]
        public float bulletScale = 0.2f;

        public Rect cornerRect;
        
        private CameraView cameraView;
        void Start()
        {
            cameraView = gameObject.GetComponent<CameraView>();
            
            Instance = this;
            EntityManager entityManager = World.Active.EntityManager;

            EntityArchetype dragonEntityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(PlayerInput),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Rotation),
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
            Entity entity = entityManager.CreateEntity(dragonEntityArchetype);
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = meshDragon,
                material = materialDragon,
                castShadows = ShadowCastingMode.On,
                receiveShadows = true
            });
            
//            Entity weaponEntity = entityManager.CreateEntity(weaponEntityArchetype);
            //CreateBullet();
        }

        private void Update()
        {
            if (cameraView)
                cornerRect = cameraView.rect;
        }
    }
}
