using UnityEngine;
using UnityEngine.UI;

/**
 * This script is for triggering players when they hit a checkpoint
 */
namespace Complete {


    public class Checkpoint : MonoBehaviour {

        public float spriteBlinkingTimer = 0.0f;
        public float spriteBlinkingMiniDuration = 0.1f;
        public float spriteBlinkingTotalTimer = 0.0f;
        public float spriteBlinkingTotalDuration = 1.0f;
        public bool startBlinking = false;

        private Image m_FillImage;
        private Canvas m_Canvas;

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
                Color playerColor = player.HitCheckpoint(transform);    // Notify the player that he stepped on you (how rude...)

                if(playerColor != Color.clear)
                {
                    startBlinking = true;
                    m_FillImage.color = playerColor;
                }
            }
        }

        private void Update()
        {
            if (startBlinking == true)
            {
                SpriteBlinkingEffect();
            }
        }

        private void SpriteBlinkingEffect()
        {
            spriteBlinkingTotalTimer += Time.deltaTime;
            if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
            {
                startBlinking = false;
                spriteBlinkingTotalTimer = 0.0f;
                m_FillImage.color = Color.white;
                m_Canvas.enabled = true;
                return;
            }

            spriteBlinkingTimer += Time.deltaTime;
            if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
            {
                spriteBlinkingTimer = 0.0f;
                m_Canvas.enabled = !m_Canvas.enabled;
            }
        }

    }
}
