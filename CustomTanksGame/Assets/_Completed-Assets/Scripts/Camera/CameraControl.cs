using UnityEngine;

namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        public bool m_SplitCamera = false;              // The flag for enabling/disabling splitted camera

        private Transform m_Target;
        private GameObject m_MaskPivot;
        private ViewBehaviour m_Behaviour;
        private bool m_isViewSplit;

        private void Awake()
        {
            m_isViewSplit = true;
            m_Behaviour = new SplitView(this, m_Target);
        }

        public void ChangeToSplitView()
        {
            if (!m_Behaviour.GetViewClass().Equals(ViewBehaviour.E_VIEWCLASS.Split))
            {
                m_isViewSplit = true;
                m_Behaviour = new SplitView(this, m_Target);
            }
        }

        public void ChangeToSingleView()
        {
            if (!m_Behaviour.GetViewClass().Equals(ViewBehaviour.E_VIEWCLASS.Single))
            {
                m_isViewSplit = false;
                m_Behaviour = new SingleView(this, m_Target);
            }
        }
        public void LateUpdate()
        {

        }

        private void FixedUpdate()
        {
            m_Behaviour.Update(m_DampTime);
        }

        public void SetTarget(Transform target)
        {
            if (target != null) m_Target = target;
            m_Behaviour.SetTarget(target);
        }

        public Transform GetTarget()
        {
            return m_Target;
        }

        public void SetStartPositionAndSize()
        {
            if (gameObject.activeSelf) m_Behaviour.Initialize();
        }

        public bool IsViewSplit()
        {
            return m_isViewSplit;
        }

        /*
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
        */

    }
}