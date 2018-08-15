using UnityEngine;

/**
 * This class is used to define the single view camera behaviour (almost the same as the original Tanks game)
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

        protected override void Move( float DampTime )
        {
            // Find the desired position.
            Vector3 target = CalculateNewPosition();

            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, target, ref m_MoveVelocity, DampTime);
        }

        protected override Vector3 CalculateNewPosition()
        {
            // Find the average position of the targets.
            Vector3 target = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // Check if we're not seeing some world bounds
            CheckBounds(ref target);

            return target;
        }
    }
}
