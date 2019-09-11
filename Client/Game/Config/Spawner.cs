using UnityEngine;
using Framework;

namespace ECS
{
    [RequireComponent(typeof(MarkPoint))]
    public class Spawner : MonoBehaviour
    {
        public Mesh Mesh;

        public Material Material;

        public float MaxHp;

        public float Damage;
    }
}