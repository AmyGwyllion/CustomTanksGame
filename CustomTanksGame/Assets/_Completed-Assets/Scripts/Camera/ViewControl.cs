using UnityEngine;

namespace Complete
{
    public class ViewControl : MonoBehaviour
    {

        private ViewBehaviour m_Behaviour;

        // Use this for initialization
        void Start()
        {
            m_Behaviour = new SingleView(gameObject);
            Debug.Log(gameObject);
        }

        public void ChangeToSplitView()
        {
            m_Behaviour = new SplitView(gameObject);
        }

        public void ChangeToSingleView()
        {
            m_Behaviour = new SingleView(gameObject);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_Behaviour.FixedUpdate();
        }

    }
}
