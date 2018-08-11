using UnityEngine;
namespace Complete
{
    public abstract class ViewBehaviour
    {
        protected CameraControl m_CameraControl;    // The CameraControl that is attatched
        protected Vector3 m_MoveVelocity;           // Reference velocity for the smooth damping of the position
        protected float m_ZoomSpeed;                // Reference zoom speed for the smooth damping of the zoom
        protected Camera m_Camera;                  // The camera we want to manipulate
        protected Transform m_Target;               // Camera player target
        protected MaskControl m_Mask;               // Camera Mask


        public ViewBehaviour(CameraControl cameraControl, Transform target) {
            m_CameraControl = cameraControl;
            m_MoveVelocity = Vector3.zero;
            m_ZoomSpeed = 0.0f;
            m_Camera = cameraControl.GetComponentInChildren<Camera>();
            m_Target = target;
            m_Mask = cameraControl.GetComponentInChildren<MaskControl>();
        }

        public virtual void Initialize(float DampTime) { Debug.Log("Reached ViewBehaviour Initialize"); }
        public virtual void Move(float DampTime) { Debug.Log("Reached ViewBehaviour Move()"); }
        public virtual void Zoom(float DampTime) { Debug.Log("Reached ViewBehaviour Zoom()"); }

        public virtual void Update(float DampTime)
        {
            Move(DampTime);
            Zoom(DampTime);
        }

        public void SetTarget(Transform target)
        {
            if(target!=null) m_Target = target;
        }
    }
}