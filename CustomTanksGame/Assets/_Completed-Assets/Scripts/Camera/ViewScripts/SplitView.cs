using UnityEngine;
namespace Complete
{
    public class SplitView : ViewBehaviour
    {
        private float m_Angle;
        public SplitView(CameraControl cameraControl, Transform target) : base(cameraControl, target)
        {
            m_CameraControl = cameraControl;
            m_Angle = 0.0f;
            if (m_Mask != null)
            {
                m_Mask.gameObject.SetActive(true);
            }
        }

        public override void Initialize(float DampTime)
        {
            // Find the desired position.
            // Set the camera's position to the desired position without damping.
            m_CameraControl.transform.position = m_Target.position;

            // Find and set the required size of the camera.
            m_Camera.orthographicSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
        }

        public override void Update (float DampTime)
        {
            base.Update(DampTime);
        }

        public override void Move( float DampTime)
        {
            // Smoothly transition to that position.
            Vector3 pos = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition() - m_Target.transform.position;
            Vector3 finalpos = m_Target.position + pos/4;
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, finalpos, ref m_MoveVelocity, DampTime);
        }

        public override void Zoom( float DampTime)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            //float requiredSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
            //m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, DampTime);
        }
    }
}