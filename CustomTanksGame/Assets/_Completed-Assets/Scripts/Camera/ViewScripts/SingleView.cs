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

        public override void Move( float DampTime )
        {
            // Find the desired position.
            Vector3 target = calculateNewPosition();

            // Check if we're not seeing some world bounds
            checkForBounds(ref target);

            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, target, ref m_MoveVelocity, DampTime);
        }

        protected override Vector3 calculateNewPosition()
        {
            // Find the average position of the targets.
            Vector3 target = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // Check if we're not seeing some world bounds
            checkForBounds(ref target);

            return target;
        }
    }
}
