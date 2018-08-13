using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankCompass : MonoBehaviour {

        [HideInInspector] public int m_PlayerNumber;
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_Color = Color.green;                 // The arrow color

        private Transform[] m_Checkpoints;                   // All the checkpoints
        private int m_Next;                                  // Next checkpoint to be reached
    
        // Use this for initialization
        void Start () {
            m_Next = 0;
        }
	
	    // Update is called once per frame
	    void Update () {
            PointToCheckPoint();
            ResizeArrow();

        }

        private void PointToCheckPoint() {
            //Here the arrow rotates to point the next checkpoint in the map
            Vector3 to = m_Checkpoints[m_Next].position;
            Vector3 from = m_Slider.transform.position;
            Vector3 v = to - from;
            v.Normalize();

            float m_Angle = (Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg);

            Vector3 rot = m_Slider.transform.eulerAngles;
            Vector3 newRot = new Vector3(rot.x, m_Angle, rot.z);

            m_Slider.transform.eulerAngles = newRot;
        }

        private void ResizeArrow() {
            //Here whe resize the arrow with the slider values if the target is getting close by a factor of distance/2
            float distance = (m_Checkpoints[m_Next].position - m_Slider.transform.position).magnitude;
            distance = distance / 2;
            distance = Mathf.Clamp(distance, m_Slider.minValue, m_Slider.maxValue);

            m_Slider.value = distance;
        }

        public bool HitCheckpoint(Transform checkpoint)
        {
            bool toRet = false;
            if (m_Checkpoints[m_Next] == checkpoint)
            {
                GetNextCheckpoint();
                toRet = true;
            }

            return toRet;
        }

        private void GetNextCheckpoint()
        {
            if (m_Next+1 >= m_Checkpoints.Length)
            {
                m_Next = 0;
                LapCompleted();
            }
            else m_Next++;
        }

        private void LapCompleted()
        {
            //gameObject.FindObject<GameManager>().AddLap(m_PlayerNumber);
        }

        public void SetCheckpoints(Transform[] checkpoints)
        {
            m_Checkpoints = checkpoints;
        }
    }
}
