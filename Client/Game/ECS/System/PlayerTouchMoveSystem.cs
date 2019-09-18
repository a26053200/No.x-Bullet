using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class PlayerTouchMoveSystem : ComponentSystem
    {
        private Vector2 leftFingerPos = Vector2.zero;    
        private Vector2 leftFingerLastPos = Vector2.zero;    
        private Vector2 leftFingerMoveBy = Vector2.zero;    
        public float slideMagnitudeX = 0.0f;    
        public float slideMagnitudeY = 0.0f;

        private Vector3 movement;
        
        private Vector3 touchStartPos = Vector3.zero;
            
        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                touchStartPos = Input.mousePosition;
            }else if (Input.GetMouseButton(0))
            {
                var movePos = Input.mousePosition;
                movement = movePos - touchStartPos;
                touchStartPos = Input.mousePosition;
            }
            
            Entities.ForEach((ref PlayerInput input) =>
            {
                input.movement = movement;
            });
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);    
                if(touch.phase == TouchPhase.Began)    
                {    
                    leftFingerPos = Vector2.zero;    
                    leftFingerLastPos = Vector2.zero;    
                    leftFingerMoveBy = Vector2.zero;    
                    slideMagnitudeX = 0;    
                    slideMagnitudeY = 0;    
                    //记录开始坐标点    
                    leftFingerPos = touch.position;    
                }    
                else if(touch.phase == TouchPhase.Moved)    
                {   //Unity3D教程手册：www.unitymanual.com    
                    leftFingerMoveBy = touch.position - leftFingerPos;    
                    leftFingerLastPos = leftFingerPos;    
                    leftFingerPos = touch.position;    
                    slideMagnitudeX = leftFingerMoveBy.x / Screen.width;    
                    slideMagnitudeY = leftFingerMoveBy.y / Screen.height;    
                }    
                else if(touch.phase == TouchPhase.Stationary)    
                {    
                    leftFingerLastPos = leftFingerPos;    
                    leftFingerPos = touch.position;    
                    slideMagnitudeX = 0.0f;    
                    slideMagnitudeY = 0.0f;    
                }    
                else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)    
                {    
                    slideMagnitudeX = 0.0f;    
                    slideMagnitudeY = 0.0f;    
                }
                
                Entities.ForEach((ref Translation translation, ref Player player) =>
                {
                    var pos = translation.Value;
                    pos += new float3(slideMagnitudeX, 0 , slideMagnitudeY);
                    translation.Value = pos;
                });
            }
        }

        
    }
}