using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//A simple class for animating the Play Again button text
public class HighlightText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text m_Text;         // The thext
    private Color m_Color1;     // First color transition
    private Color m_Color2;     // Second color transition

    // Sets the default color values
    private void SetDefaultColors()
    {
        m_Color1 = Color.yellow;
        m_Color2 = Color.white;
    }

    // The Play Again button game object is sleeping during until the RoundEnding() coroutine starts, so whe ned to wake him up
    private void OnEnable()
    {
        SetDefaultColors();
    }

    // When the cursor is hovering the text
    public void OnPointerEnter(PointerEventData eventData)
    {
        // We change the color to the same value to notify the player that this object is clickable
        m_Color1 = Color.green;
        m_Color2 = Color.green;
    }

    // When the cursor stops hovering the text
    public void OnPointerExit(PointerEventData eventData)
    {
        SetDefaultColors();
    }

    private void Update()
    {
        //Update the color transition making a "breathe" transition effect for catching the player's attention
        m_Text.color = Color.Lerp(m_Color1, m_Color2, Mathf.PingPong(Time.time, 1)); ;
    }

}
