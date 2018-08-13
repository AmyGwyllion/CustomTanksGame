using UnityEngine;

/**
 * This script is for triggering players when they hit a checkpoint
 */
namespace Complete { 
    public class Checkpoint : MonoBehaviour {

        // When a collider trigger this point
        private void OnTriggerEnter(Collider collider)
        {
            // If it has a TankCompass script attatched to it we can say is a player
            TankCompass player = collider.GetComponentInParent<TankCompass>();
            if (player != null)
                player.HitCheckpoint(transform); // Notify the player that he stepped on you (how rude...)
        }

    }
}
