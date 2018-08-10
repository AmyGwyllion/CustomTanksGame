using UnityEngine;

namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        [HideInInspector] public Transform[] m_Targets; // All the targets the camera needs to encompass.


        private Camera m_Camera;                        // Used for referencing the camera.
        private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
        private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
        private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

        /*[NEW]***************************/
        [HideInInspector]  public Transform m_MyTarget;
        private bool m_HasMask = false;
        GameObject m_Mask;
        /*********************************/

        private void Awake ()
        {
            m_Camera = GetComponentInChildren<Camera> ();

            //[NEW]
            foreach (Transform child in m_Camera.transform)
                if (child.tag == "CameraMask"){
                    m_Mask = child.gameObject;
                    m_HasMask = true;
                }
            //[HERE]
            if (m_HasMask) {

                //Put the side mask in the middle

                /*
                float x = m_Camera.rect.x;      //0
                float y = m_Camera.rect.y;      //0
                float w = m_Camera.rect.width;  //1
                float h = m_Camera.rect.height; //1

                
                float ratio = h / (w / 2);

                float xScale = m_Camera.orthographicSize / maskSize.x ;
                float zScale = m_Camera.orthographicSize / maskSize.y;

                Debug.Log("Plane Size: " + maskSize);
                Debug.Log("Lossy scale" + m_Mask.transform.lossyScale);
                Debug.Log("Local scale" + m_Mask.transform.localScale);
                Debug.Log("Camera Rect: "+ m_Camera.orthographicSize);
                Debug.Log("Camera Rect: " + m_Camera.transform.position);

                m_Mask.transform.localScale = new Vector3(3, 1, 3);
                m_Mask.transform.position = new Vector3 (m_Mask.transform.position.x - m_Mask.GetComponent<Renderer>().bounds.size.x / 2, m_Mask.transform.position.y + m_Mask.GetComponent<Renderer>().bounds.size.y / 2, m_Mask.transform.position.z );
                */

                /*
                Vector3 maskSize = m_Mask.GetComponent<Renderer>().bounds.size;
                float camHeight = m_Camera.orthographicSize * 2;
                float camWidth = (m_Camera.orthographicSize * m_Camera.aspect)*2;

                Vector3 scale = m_Mask.transform.localScale;

                float xScale = Mathf.Abs (camHeight * scale.x / maskSize.x) ;
                float yScale = 1;
                float zScale = Mathf.Abs (camWidth * scale.z / maskSize.z) ;

                Vector3 newScale = new Vector3(xScale, yScale, zScale);

                Debug.Log(newScale);
                Debug.Log(maskSize);

                m_Mask.transform.localScale = newScale;

                float posX = m_Mask.transform.localPosition.x;
                float posY = m_Mask.transform.localPosition.y;
                float posZ = m_Mask.transform.localPosition.z;
                maskSize = m_Mask.GetComponent<Renderer>().bounds.size;

                float W = Screen.width;
                float H = Screen.height;

                m_Mask.transform.localPosition = new Vector3(posX  - ( maskSize.x + m_ScreenEdgeBuffer)/2, posY, posZ );
                */

                //Camera Data

                //float camHeight = m_Camera.orthographicSize;
                //float camWidth = (m_Camera.orthographicSize * m_Camera.aspect);

                /*
                float camHeight = m_Camera.orthographicSize * 2.0f * Screen.width / Screen.height;
                float camWidth = m_Camera.orthographicSize* m_Camera.aspect * 2.0f * Screen.width / Screen.height;
                
                float camHeight = m_Camera.pixelHeight;
                float camWidth = m_Camera.pixelWidth;
                
                float camHeight = m_Camera.scaledPixelHeight;
                float camWidth = m_Camera.scaledPixelWidth;
                

                //Mask data
                Vector3 maskPos = m_Mask.transform.localPosition;
                Vector3 maskScale = m_Mask.transform.localScale;
                Vector3 maskSize = m_Mask.GetComponent<Renderer>().bounds.size;

                //Calculate the new scale
                Vector3 newScale = new Vector3(camHeight, 1,  camWidth/2);
                m_Mask.transform.localScale = newScale;

                maskSize = m_Mask.GetComponent<MeshFilter>().mesh.bounds.size;
                maskScale = m_Mask.transform.localScale;
                maskPos = m_Mask.transform.localPosition;
                //Calculate the new position
                m_Mask.transform.localPosition = new Vector3(maskPos.x - (maskSize.x/2), maskPos.y, maskPos.z);
                */

                /*
                //Camera visible Area Dimensions
                float camHeight = m_Camera.orthographicSize * 2.0f;
                float camWidth = camHeight * (Screen.height/Screen.width);
                Debug.Log(camWidth + "," + camHeight);

                //Mask Plane Size
                Vector3 maskSize = m_Mask.GetComponent<MeshFilter>().mesh.bounds.size;
                Debug.Log("Mask plane size:" + maskSize);

                //Mask Plane Local Scale
                Vector3 maskScale = m_Mask.transform.localScale;
                Debug.Log("Mask local scale:" + maskScale);

                //Calculate The New Scale
                float xScale = Mathf.Abs(camHeight * maskScale.x / maskSize.x);
                float yScale = 1;
                float zScale = Mathf.Abs(camWidth * maskScale.z / maskSize.z);

                Vector3 newScale = new Vector3(xScale, yScale, zScale);
                Debug.Log("New Scale:" + newScale);


                m_Mask.transform.localScale = newScale;

                Vector3 newMaskSize = m_Mask.GetComponent<MeshFilter>().mesh.bounds.size;
                Debug.Log("New Mask Size:" + newMaskSize.z*newScale.z);
                Debug.Log("Visible Area Dimensions: " + camWidth + "," + camHeight);


                */

            }
        }


        private void FixedUpdate ()
        {
            // Move the camera towards a desired position.
            //Move ();

            // Change the size of the camera based.
            //Zoom ();

            //[NEW]
            if (m_HasMask) UpdateMask();
        }

        //[NEW]
        private void UpdateMask() {

        }


        private void Move ()
        {
            // Find the average position of the targets.
            FindAveragePosition ();

            //[NEW] [DELETE LATER]
            if(m_MyTarget!=null) m_DesiredPosition = new Vector3(m_MyTarget.position.x, transform.position.y, m_MyTarget.position.z);

            // Smoothly transition to that position.
            transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
        }


        private void FindAveragePosition ()
        {
            Vector3 averagePos = new Vector3 ();
            int numTargets = 0;

            // Go through all the targets and add their positions together.
            for (int i = 0; i < m_Targets.Length; i++)
            {
                // If the target isn't active, go on to the next one.
                if (!m_Targets[i].gameObject.activeSelf)
                    continue;

                // Add to the average and increment the number of targets in the average.
                averagePos += m_Targets[i].position;
                numTargets++;
            }

            // If there are targets divide the sum of the positions by the number of them to find the average.
            if (numTargets > 0)
                averagePos /= numTargets;

            // Keep the same y value.
            averagePos.y = transform.position.y;

            // The desired position is the average position;
            m_DesiredPosition = averagePos;
        }


        private void Zoom ()
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            float requiredSize = FindRequiredSize();
            m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
        }


        private float FindRequiredSize ()
        {
            // Find the position the camera rig is moving towards in its local space.
            Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

            // Start the camera's size calculation at zero.
            float size = 0f;

            // Go through all the targets...
            for (int i = 0; i < m_Targets.Length; i++)
            {
                // ... and if they aren't active continue on to the next target.
                if (!m_Targets[i].gameObject.activeSelf)
                    continue;

                // Otherwise, find the position of the target in the camera's local space.
                Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

                // Find the position of the target from the desired position of the camera's local space.
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
            }

            // Add the edge buffer to the size.
            size += m_ScreenEdgeBuffer;

            // Make sure the camera's size isn't below the minimum.
            size = Mathf.Max (size, m_MinSize);

            return size;
        }


        public void SetStartPositionAndSize ()
        {
            // Find the desired position.
            FindAveragePosition ();

            // Set the camera's position to the desired position without damping.
            transform.position = m_DesiredPosition;

            // Find and set the required size of the camera.
            m_Camera.orthographicSize = FindRequiredSize ();
        }
    }
}