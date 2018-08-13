
using UnityEngine;

namespace Complete { 
    public class CheckPoint : MonoBehaviour {

        static Vector3 Reached;

        private void OnTriggerEnter(Collider player)
        {
            player.GetComponentInParent<TankCompass>().HitCheckpoint(transform);
        }
    }
}
