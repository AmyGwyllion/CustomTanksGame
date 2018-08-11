using UnityEngine;

namespace Complete
{
    public class MaskControl : MonoBehaviour {

        private Mesh m_MaskPlane;
        private Camera m_Camera;

        private void Awake()
        {
            m_MaskPlane = gameObject.GetComponentInChildren<MeshFilter>().mesh;
            m_Camera = gameObject.GetComponentInParent<Camera>();
        }
        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        //FixedUpdate[TODO]
        private void LateUpdate() {
            Resize();
            PointToTarget();
        }

        private void Resize()
        {

            float ch = m_Camera.orthographicSize;
            float cw = m_Camera.orthographicSize * m_Camera.aspect;

            Vector3 maskSize = m_MaskPlane.bounds.size;

            float maxHeight = (Mathf.Sqrt(ch * ch + cw * cw)) * 2;

            float desiredWidth = maxHeight / maskSize.z / 2;
            float desiredHeight = maxHeight / maskSize.x;

            Vector3 newScale = new Vector3(desiredWidth, desiredHeight, 1);

            transform.localScale = newScale;
        }

        private void PointToTarget()
        {
            //Rotate the MaskPivot to m_AveragePosition
            Vector3 from = GetComponentInParent<CameraControl>().GetTarget().position;
            Vector3 to = GetComponentInParent<CameraManager>().GetAveragePosition();

            Vector3 v = to - from;
            v.Normalize();

            float angle = Mathf.Atan2(to.z, to.x) * Mathf.Rad2Deg;

            Vector3 maskRot = transform.eulerAngles;
            Vector3 newRot = new Vector3(maskRot.x, angle, maskRot.z);

            transform.eulerAngles = newRot;

        }
    }
}
