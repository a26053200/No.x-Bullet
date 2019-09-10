using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace ECS
{
    public class PlayerInputSystem : ComponentSystem
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        protected override void OnUpdate()
        {
            Entities.ForEach((ref PlayerInput input) =>
            {
                input.Horizontal = Input.GetAxis(Horizontal);
                input.Vertical = Input.GetAxis(Vertical);
            });
        }
    }
}