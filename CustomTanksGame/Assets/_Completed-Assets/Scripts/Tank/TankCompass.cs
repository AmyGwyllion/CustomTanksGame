using UnityEngine;
using UnityEngine.UI;

public class TankCompass : MonoBehaviour {

    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_Color = Color.green;                               // The arrow color
    public Transform[] CheckPoints;                     // All the active checkpoints

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PointToCheckPoint();
	}

    private void PointToCheckPoint() {
        //Here the arrow rotates to point the next checkpoint in the map
        Vector3 to = CheckPoints[0].transform.position;
        Vector3 from = m_Slider.transform.position;
        Vector3 v = to - from;
        v.Normalize();

        float m_Angle = (Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg);

        Vector3 rot = m_Slider.transform.eulerAngles;
        Vector3 newRot = new Vector3(rot.x, m_Angle, rot.z);

        m_Slider.transform.eulerAngles = newRot;
    }
}
