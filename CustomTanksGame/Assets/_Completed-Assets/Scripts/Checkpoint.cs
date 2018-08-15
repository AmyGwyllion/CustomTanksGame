using UnityEngine;
using UnityEngine.UI;

/**
 * This script is for triggering players when they hit a checkpoint
 */
namespace Complete
{

    public class Checkpoint : MonoBehaviour
    {

        public float m_BlinkCount = 0.0f;               // The blink iteration counter
        public float m_BlinkLenth = 0.1f;               // The time we want our canvas to blink
        public float m_BlinkCounter = 0.0f;             // The total time blinking counter
        public float m_BlinkTotalDuration = 1.0f;       // The duration of the blink effect   
        public bool m_StartBlinking = false;            // A flag for triggering the blink effect
        public AudioSource m_Audio;                     // Reference to the audio source used to play the checkpoint audio
        public AudioClip m_Checkpoint;                  // The checkpoint audio cllip

        private Canvas m_Canvas;                        // The canvas object for making the blink effect enabling/disabling it
        private Image m_FillImage;                      // The checkpoint image

        // Initialize the variables
        private void Awake()
        {
            m_Canvas = gameObject.GetComponentInChildren<Canvas>();

            m_FillImage = gameObject.GetComponentInChildren<Image>();
            m_FillImage.color = Color.white;
        }

        // When a collider trigger this point
        private void OnTriggerEnter(Collider collider)
        {
            // If it has a TankCompass script attatched to it we can say is a player
            TankCompass player = collider.GetComponentInParent<TankCompass>();
            if (player != null)
            {
                // Notify the player that he stepped on you by showing his color
                Color playerColor = player.HitCheckpoint(transform);

                // If the player has a color
                if (playerColor != Color.clear)
                {
                    // Start blinking and play a nice sound effect
                    m_StartBlinking = true;
                    m_FillImage.color = playerColor;
                    m_Audio.Play();
                }
            }
        }

        private void Update()
        {
            // If someone collided and activated the blinking flag start blinking
            if (m_StartBlinking) Blink();
        }

        private void Blink()
        {
            // Add time to the counter and if we passed the total blink duration reset the variables and enable the canvas
            m_BlinkCounter += Time.deltaTime;
            if (m_BlinkCounter >= m_BlinkTotalDuration)
            {
                m_StartBlinking = false;
                m_BlinkCounter = 0.0f;
                m_FillImage.color = Color.white;
                m_Canvas.enabled = true;
                return;
            }

            // This piece of code switchs between enabling and disabling canvas every blink step
            m_BlinkCount += Time.deltaTime;
            if (m_BlinkCount >= m_BlinkLenth)
            {
                m_BlinkCount = 0.0f;
                m_Canvas.enabled = !m_Canvas.enabled;
            }
        }
    }
}
