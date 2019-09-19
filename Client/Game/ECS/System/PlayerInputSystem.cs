using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Game
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
                if (!(input.Horizontal > 0 || input.Horizontal < 0))
                    input.Horizontal = ECSWorld.Instance.Horizontal;
                input.Vertical = Input.GetAxis(Vertical);
                if (!(input.Vertical > 0 || input.Vertical < 0))
                    input.Vertical = ECSWorld.Instance.Vertical;
            });
        }
    }
}