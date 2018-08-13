
using UnityEngine;

namespace Complete { 
    public class Checkpoint : MonoBehaviour {

        public bool m_IsStartPoint;

        private void OnTriggerEnter(Collider player)
        {
            if (player.GetComponentInParent<TankCompass>().HitCheckpoint(transform))
                if (m_IsStartPoint)
                {
                    int p_number = player.GetComponent<TankCompass>().m_PlayerNumber;
                    GameObject.FindWithTag("GameManager").GetComponent<GameManager>().AddLap(p_number);
                }
        }
    }
}
