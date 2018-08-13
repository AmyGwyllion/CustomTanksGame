
using UnityEngine;

namespace Complete { 
    public class CheckPoint : MonoBehaviour {

        static Vector3 Reached;

        private void OnTriggerEnter(Collider player)
        {
            Debug.Log(player.GetComponentInParent<TankMovement>().m_PlayerNumber);
        }
    }
}
