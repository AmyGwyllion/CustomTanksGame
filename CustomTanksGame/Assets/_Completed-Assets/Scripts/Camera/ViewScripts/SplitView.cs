using UnityEngine;

/**
 * This class is used to thefine the split view camera behaviour
 */
namespace Complete
{
    public class SplitView : ViewBehaviour
    {

        private float m_Zoom = 15.0f;
        private float m_Radius = 15.0f;

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
            Vector3 target = calculateNewPosition();

            // Set the camera's position to the desired position without damping.
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
            
            // Smoothly transition to that position.
            m_CameraControl.transform.position = Vector3.SmoothDamp(m_CameraControl.transform.position, target, ref m_MoveVelocity, DampTime); 
        }

        public override void Zoom( float DampTime)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            //float requiredSize = m_CameraControl.GetComponentInParent<CameraManager>().GetRequiredSize(m_CameraControl);
            m_Camera.orthographicSize = m_Zoom;
        }

        private Vector3 calculateNewPosition()
        {
            /*
            * As long as whe are moving the centre of the camera we need to find a position between the player and 
            * the all players position average point to not lose track of the player
            * 
            * So we have a imaginary circunference around the player with radius m_Radius
            * And the direction vector from the player to the average position
            * The new camera point will be the intersection between the direction vector and the player's circunference
            */

            // All players position average point
            Vector3 average = m_CameraControl.GetComponentInParent<CameraManager>().GetAveragePosition();

            // The direction where the average point is from the player
            Vector3 target = average - m_Player.transform.position;
            Vector3 dir = target/target.magnitude;

            // The new camera position without move the Y coordinate
            target.x = m_Player.transform.position.x + dir.x * m_Radius;
            target.z = m_Player.transform.position.z + dir.z * m_Radius;

            // Check if we are not hitting some world bounds
            checkForBounds(ref target);

            return target;
        }

    }
}