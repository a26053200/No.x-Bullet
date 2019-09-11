using Framework;
using LuaInterface;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class ECSWorld : MonoBehaviour
    {
        public static ECSWorld Instance;

        public static EntityArchetype FireEntityArchetype;

        public static EntityArchetype BulletEntityArchetype;

        [SerializeField] [NoToLua]public Mesh meshAirplane;

        [SerializeField] [NoToLua]public Material materialAirplane;

        [SerializeField] [NoToLua]public Mesh meshBullet;

        [SerializeField] [NoToLua]public Material materialBullet;

        [SerializeField] private float speed;

        [SerializeField] public float bulletSpeed = 6f;

        [SerializeField] public float shootDeltaTime = 0.1f;

        [SerializeField] public float bulletScale = 0.2f;

        [SerializeField] public float2 playerSize = new float2(0.5f, 1);

        [SerializeField] public float shootOffset = 0.5f;

        public GameObjectEntity pointLightEntity;

        [HideInInspector] public Rect cornerRect;

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
                typeof(Player),
                typeof(Weapon)
            );
            FireEntityArchetype = _entityManager.CreateArchetype(typeof(Firing));

            BulletEntityArchetype = _entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Rotation),
                typeof(Scale),
                typeof(Bullet)
            );

            _entityManager.AddComponent<FireLight>(pointLightEntity.Entity);
            _entityManager.AddComponent<MoveSpeed>(pointLightEntity.Entity);
            _entityManager.SetComponentData(pointLightEntity.Entity, new MoveSpeed() {Speed = speed});
            _entityManager.AddComponent<PlayerInput>(pointLightEntity.Entity);
//            Entity weaponEntity = entityManager.CreateEntity(weaponEntityArchetype);
            //CreateBullet();
        }

        public void CreateAirplane(AirplaneInfo info)
        {
            //实体的本地数组
            Entity entity = _entityManager.CreateEntity(_airplaneEntityArchetype);
            _entityManager.SetComponentData(entity, new MoveSpeed() {Speed = speed});
            _entityManager.SetComponentData(entity, new Translation()
            {
                Value = new float3(0,0,cornerRect.y - cornerRect.height + playerSize.y)
            });
            _entityManager.SetComponentData(entity, new Scale() {Value = 0.5f});
            _entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = info.Mesh,
                material = info.Material,
                castShadows = info.ShadowCastingMode,
                receiveShadows = info.ReceiveShadows
            });
        }
        private void Update()
        {
            if (_cameraView)
                cornerRect = _cameraView.rect;
        }
    }

    
}