using UnityEngine;
using UnityEngine.UI;

/**
 * This script controls the status UI circle under the tank 
 */
namespace Complete
{
    public class TankStatus : MonoBehaviour
    {

        [HideInInspector] public int m_PlayerNumber;        // The number of the player associated, set by TankManager
        
        public Slider m_Slider;                             // The slider to represent the player's quiet status time left
        public Image m_FillImage;                           // The image component of the slider
        public Color m_QuietColor = Color.gray;            // The color the health bar will be when is quiet
        public Color m_NotQuietColor = Color.green;        // The color the health bar will be when is not quiet

        private bool m_IsQuiet;                             // Flag for knowing if the player is in a quiet state
        private float m_QuietTimeCount;                     // Timer for counting the seconds this player is going to be quiet
        private float m_TotalQuietTime;                     // The initial time this player was told to be quiet

        private void OnEnable()
        {
            m_IsQuiet = false;
            
            // Set the slider default min, max and default values
            m_Slider.minValue = 0.0f;
            m_Slider.maxValue = 100.0f;
            m_Slider.value = m_Slider.maxValue;

            // When the tank is enabled, reset the tank's quiet time count and the total quiet time
            m_QuietTimeCount = 0.0f;
            m_TotalQuietTime = 0.0f;

            // Update the slider's value and color
            SetTankStatusUI();
        }

        private void SetTankStatusUI()
        {
            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp (m_QuietColor , m_NotQuietColor, m_Slider.value / m_Slider.maxValue);
        }

        public void StayQuiet(float time)
        {
            // If player is not already quiet (we don't want to be quiet for a long time!)
            if (!m_IsQuiet)
            {
                m_IsQuiet = true;

                // Set the counter and the max time to the value passed by reference
                m_TotalQuietTime = time;
                m_QuietTimeCount = time;

                // Ask the GameManager to disable the movement (but not the UI elements) of the associate player
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().DisablePlayerMovement(m_PlayerNumber);
            }
        }

        public void Update()
        {
            // Get the world delta time
            float time = Time.deltaTime;

            //If the player is quiet
            if (m_IsQuiet && m_QuietTimeCount > 0.0f)
            {
                // Decrease the quiet time counter
                m_QuietTimeCount -= time;

                // Update the slider value (per relative percentage)
                m_Slider.value = m_Slider.maxValue - m_QuietTimeCount * (m_Slider.maxValue / m_TotalQuietTime);
                m_FillImage.color = Color.Lerp(m_QuietColor, m_NotQuietColor, m_Slider.value / m_Slider.maxValue);
            }
            // If the player is not quiet anymore
            else if(m_IsQuiet && m_QuietTimeCount <= 0)
            {
                m_IsQuiet = false;

                // Reset the quiet max time value...
                m_QuietTimeCount = 0.0f;

                // ...and tell the GameManager the player is not quiet anymore!
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().EnablePlayerMovement(m_PlayerNumber);
            }
        }

    }
}