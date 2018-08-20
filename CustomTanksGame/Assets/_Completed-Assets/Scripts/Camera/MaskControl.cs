using UnityEngine;
using UnityEngine.UI;
/**
 * This script is used to control the plane mask for the split camera view 
 */
namespace Complete
{
    public class MaskControl : MonoBehaviour {

        private Mesh m_MaskPlane;       // The mask plane reference
        private Camera m_Camera;        // The camera attatched
        private Slider m_SplitLine;     // The line that shows the split view bounds and the distance between players

        private void Awake()
        {
            //Initialize the values
            m_SplitLine = gameObject.GetComponentInChildren<Slider>();
            m_MaskPlane = gameObject.GetComponentInChildren<MeshFilter>().mesh;
            m_Camera = gameObject.GetComponentInParent<Camera>();
        }

        // We want to update the mask after the players update
        private void LateUpdate() {
            //This function resizes the plane to fit the camera view
            Resize();

            // This function rotates the plane for being always looking at the players average point
            PointToTarget();
        }

        // This funcion updates the plane size according to the camera size
        private void Resize()
        {
            // Get the camera dimensions
            float cameraHeight = m_Camera.orthographicSize;
            float cameraWith = m_Camera.orthographicSize * m_Camera.aspect;

            // Get the plane mesh size
            Vector3 maskSize = m_MaskPlane.bounds.size;

            // Calculate the max height for rotation, this is the hypontenuse multiplied by two because the size value of the ortografic camera is only half of the dimensions
            float maxHeight = (Mathf.Sqrt(cameraHeight * cameraHeight + cameraWith * cameraWith)) * 2;

            // We want the plane to be half the width and the max height in the plane's scale, so we divide the vaules by the scale
            float desiredWidth = maxHeight / maskSize.z / 2;
            float desiredHeight = maxHeight / maskSize.x;


            // Assign the final scale
            transform.localScale = new Vector3(desiredWidth, desiredHeight, 1.0f);
        }

        // This function updates the plane mask to be facing the average position
        private void PointToTarget()
        {
            // Rotate the MaskPivot to m_AveragePosition
            // Get the distance between the player and the average point of all players position
            Vector3 from = GetComponentInParent<CameraControl>().GetTarget().position;
            Vector3 to = GetComponentInParent<CameraManager>().GetAveragePosition();
            Vector3 distance = to - from;

            // Get the camera rotation in Y coordinate
            float cameraRot = GetComponentInParent<CameraControl>().transform.eulerAngles.y;

            // Update the UI split line before normalizing the distance vector
            // The line grows depending on the distance of the players by a 1/2 factor
            m_SplitLine.value = Mathf.Clamp(distance.magnitude/2, m_SplitLine.minValue, m_SplitLine.maxValue);

            distance.Normalize();

            // Calculate the arc tangent between the coordinates, pass it to radians and add the camera Y axis rotation
            float newAngle = (Mathf.Atan2(distance.z, distance.x) * Mathf.Rad2Deg) + cameraRot;

            // Rotate the mask in Z axis only
            Vector3 maskRot = transform.eulerAngles;
            Vector3 newRot = new Vector3(maskRot.x, maskRot.y, newAngle);

            transform.eulerAngles = newRot;
        }

    }
}
