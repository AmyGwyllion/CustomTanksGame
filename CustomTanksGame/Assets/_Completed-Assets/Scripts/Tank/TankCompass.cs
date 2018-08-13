using UnityEngine;
using UnityEngine.UI;

public class TankCompass : MonoBehaviour {

    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_Color = Color.green;                               // The arrow color
    public Transform[] CheckPoints;                     // All the targets the 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PointToCheckPoint() {
        //Here the arrow rotates to point the next checkpoint in the map
    }
}
