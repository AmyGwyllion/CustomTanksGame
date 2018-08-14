using UnityEngine;
using UnityEngine.UI;
/**
 * This script is used to control the plane mask for the split camera view 
 */
namespace Complete
{
    public class MaskControl : MonoBehaviour {

        private Mesh m_MaskPlane;       // The mask plane reference
        private Camera m_Camera;        // The camera indeed
        private float m_Angle;          // The pivot rotation angle
        private Slider m_SplitLine;            // The line that shows the camera bounds and the distance between players

        private void Awake()
        {
            m_SplitLine = gameObject.GetComponentInChildren<Slider>();
            m_MaskPlane = gameObject.GetComponentInChildren<MeshFilter>().mesh;
            m_Camera = gameObject.GetComponentInParent<Camera>();
        }

        private void FixedUpdate() {
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
            float cameraRot = GetComponentInParent<CameraControl>().transform.eulerAngles.y;

            Vector3 distance = to - from;

            m_SplitLine.value = Mathf.Clamp(distance.magnitude/2, m_SplitLine.minValue, m_SplitLine.maxValue);

            distance.Normalize();

            m_Angle = (Mathf.Atan2(distance.z, distance.x) * Mathf.Rad2Deg) + cameraRot;

            Vector3 maskRot = transform.eulerAngles;
            Vector3 newRot = new Vector3(maskRot.x, maskRot.y, m_Angle);

            transform.eulerAngles = newRot;


        }

        public float GetRotationAngle()
        {
            return m_Angle;
        }
    }
}
