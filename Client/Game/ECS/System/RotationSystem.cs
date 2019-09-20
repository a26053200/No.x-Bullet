using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
//    public class RotationSystem : ComponentSystem
//    {
//        protected override void OnUpdate()
//        {
//            Entities.ForEach((ref Translation translation ,ref PlayerInput input ,ref Player player) =>
//            {
//                if (Input.GetMouseButton(0) || Input.touchCount>0 && Input.GetTouch(0).phase== TouchPhase.Moved)
//                {
//                    var touchPosition = Input.mousePosition;
//                    if (Camera.main != null)
//                    {
//                        var cameraRay = Camera.main.ScreenPointToRay(touchPosition);
//                        var layerMask = LayerMask.GetMask("Floor");
//                        if (Physics.Raycast(cameraRay, out var hit, 100, layerMask))
//                        {
//                            var forward = hit.point - (Vector3)translation.Value;
//                            var rotation = Quaternion.LookRotation(forward);
//                            input.Rotation = new Quaternion(0,rotation.y,0, rotation.w);
//                        }
//                    }
//                }
//            });
//        }
//    }
}