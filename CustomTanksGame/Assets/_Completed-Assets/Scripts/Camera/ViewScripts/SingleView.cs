using UnityEngine;
namespace Complete
{
    public class SingleView : ViewBehaviour
    {
        private Vector3 m_AveragePosition;

        public SingleView(CameraControl cameraControl, Transform target) : base(cameraControl, target)
        {
            m_CameraControl = cameraControl;
            m_AveragePosition = Vector3.zero;
        }

        public override void Initialize(float DampTime)
        {
            // Find the desired position.
            // Set the camera's position to the desired position without damping.
            m_CameraControl.transform.position = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // Find and set the required size of the camera.
            m_Camera.orthographicSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);

        }

        public override void Update(float DampTime)
        {
            base.Update(DampTime);
        }

        public override void Move( float DampTime )
        {
            // Find the average position of the targets.
            m_AveragePosition = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, m_AveragePosition, ref m_MoveVelocity, DampTime);
        }

        public override void Zoom(float DampTime)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            float requiredSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
            m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, DampTime);
        }
    }
}
