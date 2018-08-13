using UnityEngine;
using UnityEngine.UI;

/**
 * This script controls the UI arrow under the tank 
 */
namespace Complete
{
    public class TankCompass : MonoBehaviour
    {

        [HideInInspector] public int m_PlayerNumber;        // The number of the player associated, set by TankManager

        public Slider m_Slider;                             // The slider to represent how far is the player from the checkpoint
        public Image m_FillImage;                           // The arrow image component of the slider
        public Color m_Color = Color.green;                 // The arrow color

        private Transform[] m_Checkpoints;                  // All the checkpoints, set by TankManager
        private int m_Next;                                 // Next checkpoint index to be reached

        void Start()
        {
            // Reset the next checkpoint
            m_Next = 0;
        }

        public void FixedUpdate()
        {
            // Update where the arrow is pointing to
            PointToCheckpoint();

            // Resize the arrow lenght in his limits, representing the distance from the checkpoint
            ResizeArrow();
        }

        private void PointToCheckpoint()
        {
            // First get the direction vector
            Vector3 to = m_Checkpoints[m_Next].position;
            Vector3 from = m_Slider.transform.position;
            Vector3 v = to - from;
            v.Normalize();

            // Then get his arc tangent and pass from radians to angles
            float m_Angle = (Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg);

            // Get the slider actual rotation in angles
            Vector3 rot = m_Slider.transform.eulerAngles;

            // Rotate the arrow only in X and Z axis
            Vector3 newRot = new Vector3(rot.x, m_Angle, rot.z);
            m_Slider.transform.eulerAngles = newRot;
        }

        private void ResizeArrow()
        {
            //Here whe resize the arrow with the slider values if the target is getting close by a simple factor of distance/2
            float factor = 1.0f / 2.0f;

            // First whe get the actual distance
            float distance = (m_Checkpoints[m_Next].position - m_Slider.transform.position).magnitude;

            // Now we calculate the new distance and clamp it to the slider min and max values
            distance = distance / factor;
            distance = Mathf.Clamp(distance, m_Slider.minValue, m_Slider.maxValue);

            // Update the value
            m_Slider.value = distance;
        }

        // This function is triggered when a checkpoint collides with the player, triggered by Checkpoint script
        public bool HitCheckpoint(Transform checkpoint)
        {
            // If the checkpoint is the correct
            if (m_Checkpoints[m_Next] == checkpoint)
            {
                //Get the next checkpoint from the array
                GetNextCheckpoint();
                return true;
            }

            // If the checkpoint is not the correct one
            return false;
        }

        private void GetNextCheckpoint()
        {
            // If the next checkpoint index goes off the list...
            if (m_Next + 1 >= m_Checkpoints.Length)
            {
                // ...we've completed a lap! Tell GameManager to increase our lap dial and reset the next lap index
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().AddLap(m_PlayerNumber);
                m_Next = 0;
            }
            // Else increment the next checkpoint index
            else m_Next++;
        }

        // This function is used by the TankManager to set the checkpoint list, so we dont have to make the variable public
        public void SetCheckpoints(Transform[] checkpoints)
        {
            m_Checkpoints = checkpoints;
        }

    }
}
