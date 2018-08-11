using UnityEngine;

namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.

        private Camera m_Camera;                        // Used for referencing the camera.
        private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
        private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
        private Vector3 m_AveragePosition;              // The position the camera is moving towards.

        /*[NEW]***************************/
        [HideInInspector]  public Transform m_Target;
        //private bool m_HasMask = false;
        private GameObject m_MaskPivot;
        //private Transform m_Mask;
        private ViewControl m_Behaviour;
        private MaskControl m_Mask;
        /*********************************/

        private void Awake ()
        {
            m_Camera = GetComponentInChildren<Camera> ();
            m_Behaviour = GetComponent<ViewControl>();
            m_Mask = GetComponentInChildren<MaskControl>();
        }

        private void Start()
        {
            m_Behaviour.ChangeToSplitView();
            
        }

        private void FixedUpdate ()
        {
            // Move the camera towards a desired position.
            Move ();

            // Change the size of the camera based.
            Zoom ();

        }

        private void Move ()
        {
            // Find the average position of the targets.
            m_AveragePosition = GetComponentInParent<CameraManager>().GetAveragePosition();

            // Smoothly transition to that position.
            //transform.position = Vector3.SmoothDamp(transform.position, m_AveragePosition, ref m_MoveVelocity, m_DampTime);

            //Follow the player
            Vector3 newPosition = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref m_MoveVelocity, m_DampTime);
        }

        private void Zoom ()
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            float requiredSize = GetComponentInParent<CameraManager>().GetRequiredSize(this);
            m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
        }

        public void SetStartPositionAndSize ()
        {
            // Find the desired position.
            //FindAveragePosition ();
            m_AveragePosition = GetComponentInParent<CameraManager>().GetAveragePosition();

            // Set the camera's position to the desired position without damping.
            transform.position = m_AveragePosition;

            // Find and set the required size of the camera.
            m_Camera.orthographicSize = GetComponentInParent<CameraManager>().GetRequiredSize(this);
        }

        /****SETTERS****/
        public void SetTarget(Transform target)
        {
            if(target!=null) m_Target = target;
        }

        public float GetAspectRatio()
        {
            if (m_Camera != null) return m_Camera.aspect;
            else return -1.0f;
        }
        
    }
}