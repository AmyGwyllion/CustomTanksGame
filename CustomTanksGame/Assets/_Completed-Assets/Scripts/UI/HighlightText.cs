using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text m_Text;
    private bool m_Hover;
    private Color m_Color1;
    private Color m_Color2;

    private void OnEnable()
    {
        m_Hover = false;
        m_Color1 = Color.yellow;
        m_Color2 = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Color1 = Color.green;
        m_Color2 = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Color1 = Color.yellow;
        m_Color2 = Color.white;
    }

    private void Update()
    {
        m_Text.color = Color.Lerp(m_Color1, m_Color2, Mathf.PingPong(Time.time, 1)); ;
    }

}
