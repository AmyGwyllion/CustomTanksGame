using UnityEngine;

/**
 * This class is used to thefine the single view camera behaviour
 */
namespace Complete
{
    public class SingleView : ViewBehaviour
    {
        // Class constructor
         public SingleView(CameraControl cameraControl, Transform target) : base(cameraControl, target)
        {
            m_Class = E_VIEWCLASS.Single;                               // The object class identifier
            m_CameraControl = cameraControl;                            // The CameraControl object attatched
            if (m_Mask != null) m_Mask.gameObject.SetActive(false);     // If the object attatched has a mask disable it
        }

        // We call this method for initializing the camera position without damping time
        public override void Initialize()
        {
            // Find the desired position.
            Vector3 target = calculateNewPosition();
            
            // Set the camera's position to the desired position without damping.
            m_CameraControl.transform.position = target;

            // Find and set the required size of the camera.
            m_Size = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);

            m_Size = Mathf.Clamp(m_Size, m_MinSize, m_MaxSize);

            m_Camera.orthographicSize = m_Size;
        }

        public override void Update(float DampTime)
        {
            //Use the base class update
            base.Update(DampTime);
        }

        public override void Move( float DampTime )
        {
            // Find the desired position.
            Vector3 target = calculateNewPosition();

            // Check if we're not seeing some world bounds
            checkForBounds(ref target);

            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, target, ref m_MoveVelocity, DampTime);
        }

        public override void Zoom(float DampTime)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            m_Size = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);

            m_Size = Mathf.Clamp(m_Size, m_MinSize, m_MaxSize);

            m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, m_Size, ref m_ZoomSpeed, DampTime);
        }

        private Vector3 calculateNewPosition()
        {
            // Find the average position of the targets.
            Vector3 target = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // Check if we're not seeing some world bounds
            checkForBounds(ref target);

            return target;
        }
    }
}
