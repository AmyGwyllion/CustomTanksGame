using UnityEngine;

/**
 * We use this class to make track of the camera behaviour 
 */
namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.

        private Transform m_Target;                     // The target we want to keep track 
        private ViewBehaviour m_Behaviour;              // The camera view behaviour

        private void Awake()
        {
            m_Behaviour = new SplitView(this, m_Target);
        }

        public void SetStartPositionAndSize()
        {
            if (gameObject.activeSelf)
                m_Behaviour.Initialize();
        }

        private void FixedUpdate()
        {
            m_Behaviour.Update(m_DampTime);
        }

        public void ChangeToSplitView()
        {
            if(!m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Split))
                m_Behaviour = new SplitView(this, m_Target);
        }

        public void ChangeToSingleView()
        {
            if (!m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Single))
                m_Behaviour = new SingleView(this, m_Target);
        }

        public bool IsViewSplit()
        {
            return m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Split) ? true : false;
        }

        public void SetTarget(Transform target)
        {
            if (target != null)
                m_Target = target;

            m_Behaviour.SetTarget(target);
        }

        public Transform GetTarget()
        {
            return m_Target;
        }
    }
}