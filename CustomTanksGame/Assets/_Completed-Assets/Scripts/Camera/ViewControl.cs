using UnityEngine;

public class ViewControl : MonoBehaviour{

    private ViewBehaviour m_Behaviour;

    // Use this for initialization
    void Start () {
        m_Behaviour = new ViewBehaviour();
	}

    public void ChangeToSplitBehaviour() {
        m_Behaviour = new SplitView();
    }

    public void ChangeToSingleBehaviour() {
        m_Behaviour = new SingleView();
    }

	// Update is called once per frame
	void Update () {
        m_Behaviour.Update();
    }

}
