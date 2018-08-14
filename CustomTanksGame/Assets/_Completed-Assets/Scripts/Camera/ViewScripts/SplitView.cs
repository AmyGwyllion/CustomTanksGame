using UnityEngine;

/**
 * This class is used to thefine the split view camera behaviour
 */
namespace Complete
{
    public class SplitView : ViewBehaviour
    {
        // Class constructor
        public SplitView(CameraControl cameraControl, Transform target) : base(cameraControl, target)
        {
            m_Class = E_VIEWCLASS.Split;                            // The object class identifier
            m_CameraControl = cameraControl;                        // The CameraControl object attatched to it
            if (m_Mask != null) m_Mask.gameObject.SetActive(true);  // If the camera has a mask on it enable it
        }

        // We call this method for initializing the camera position without damping time
        public override void Initialize()
        {
            // Find the desired position.
            // Set the camera's position to the desired position without damping.

            Vector3 target = calculateNewPosition();
            checkForBounds(ref target);

            m_CameraControl.transform.position = target;

            // Find and set the required size of the camera.
            m_Camera.orthographicSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
        }

        public override void Update (float DampTime)
        {
            //Use the base class update
            base.Update(DampTime);
        }

        public override void Move( float DampTime)
        {
            // Find the desired camera position
            Vector3 target = calculateNewPosition();
            checkForBounds(ref target);
            
            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, target, ref m_MoveVelocity, DampTime); 
        }

        private Vector3 calculateNewPosition()
        {
            Vector3 average = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();
            Vector3 target = average - m_Player.transform.position;
            float radius = 10.0f;
            Vector3 dir = target/ target.magnitude;

            target.x = m_Player.transform.position.x + dir.x * radius;
            target.z = m_Player.transform.position.z + dir.z * radius;
            return target;
        }

        public override void Zoom( float DampTime)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            //float requiredSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
            m_Camera.orthographicSize = 10.0f;
        }
    }
}