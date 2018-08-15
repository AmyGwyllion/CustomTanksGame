using UnityEngine;

/**
 * We use this class to make track of the camera behaviour 
 */
namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.

        private Transform m_Target;                     // The target we want to keep track 
        private ViewBehaviour m_Behaviour;              // The camera view behaviour

        private void Awake()
        {
            // Initialize our first game view
            m_Behaviour = new SplitView(this, m_Target);
        }

        public void SetStartPositionAndSize()
        {
            // If this camera control is activated initialize the view behaviour
            if (gameObject.activeSelf)
                m_Behaviour.Initialize();
        }

        private void FixedUpdate()
        {
            // Update the actual camrea behaviour
            m_Behaviour.Update(m_DampTime);
        }

        // This function switches the actual view behaviour to a split screen view behaviour...
        public void ChangeToSplitView()
        {
            // ...only if we are not already in a split behaviour!
            if(!m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Split))
                m_Behaviour = new SplitView(this, m_Target);
        }

        // This function switches the actual view behaviour to a single view behaviour...
        public void ChangeToSingleView()
        {
            // ...only if we are not already in a single view behaviour
            if (!m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Single))
                m_Behaviour = new SingleView(this, m_Target);
        }

        // Check if this camera is on a split behaviour
        public bool IsViewSplit()
        {
            return m_Behaviour.GetClass().Equals(ViewBehaviour.E_VIEWCLASS.Split) ? true : false;
        }

        // Set this camera target
        public void SetTarget(Transform target)
        {
            if (target != null)
                m_Target = target;

            m_Behaviour.SetTarget(target);
        }

        // Returns this camera target
        public Transform GetTarget()
        {
            return m_Target;
        }
    }
}