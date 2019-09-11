using Unity.Entities;
using UnityEngine;

namespace ECS
{
    public class FireLightSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Light light, ref FireLight fireLight) =>
            {
                if (fireLight.StartTime > 0 && Time.time - fireLight.StartTime > 0.06f)
                {
                    fireLight.StartTime = 0;
                    light.enabled = false;
                }
                light.enabled = fireLight.StartTime > 0;
                var pos = fireLight.pos;
                pos.z += 1;
                light.transform.position = pos;
            });
        }
    }
}