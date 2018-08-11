using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete { 
    public class CameraManager : MonoBehaviour {

        private TankManager[] m_Players;                         // All player objects in scene
        private Transform[] m_PTransforms;                       // All player transforms
        private List<CameraControl> m_Cameras;                   // All Cameras Controllers
        private Dictionary<int, CameraControl> m_PlayerCamera;   // Player asigned camera

        private void Awake()
        {
            //Initialize variables
            m_Players = new TankManager[0];
            m_PTransforms = new Transform[0];
            m_Cameras = new List<CameraControl>();
            m_PlayerCamera = new Dictionary<int, CameraControl>();   
        }

        // Use this for initialization
        void Start ()
        {
            //Get all camera childs
            foreach (Transform child in transform) m_Cameras.Add(child.gameObject.GetComponent<CameraControl>());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckP1P2Cameras();
        }

        //If their camera bounds are close merge view
        private void CheckP1P2Cameras()
        {
            Camera P1 = m_PlayerCamera[1].GetComponentInChildren<Camera>();
            Camera P2 = m_PlayerCamera[2].GetComponentInChildren<Camera>();
            float dist = Vector3.Distance(m_PTransforms[0].position, m_PTransforms[1].position);
            Vector3 coord = P1.WorldToViewportPoint(m_PTransforms[1].position);

            if (coord.x > 0.0f && coord.x < 1.0f || coord.y > 0.0f && coord.y < 1.0f)
            {
                //Merge Cameras
                if (m_PlayerCamera[1].IsViewSplit())
                {
                    m_PlayerCamera[1].ChangeToSingleView();
                    m_PlayerCamera[2].ChangeToSingleView();
                }
            }
            //Else if zoom is to high
            if(dist>40.0f)
            {
                if (!m_PlayerCamera[1].IsViewSplit())
                {
                    m_PlayerCamera[1].ChangeToSplitView();
                    m_PlayerCamera[2].ChangeToSplitView();
                }
            }
           
        }

        /****PUBLIC****/
        public void Initialize()
        {
            // Create a collection of transforms the same size as the number of tanks.
            m_PTransforms = new Transform[m_Players.Length];

            // For each of these transforms...
            for (int i = 0; i < m_PTransforms.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                m_PTransforms[i] = m_Players[i].m_Instance.transform;

            }

            for (int i = 0; i < m_Cameras.Count; i++) {
                m_Cameras[i].SetStartPositionAndSize();
            }
        }

        public void SetCameraTarget()
        {
            for (int i = 0; i < m_Players.Length && i< m_Cameras.Count; i++)
            {
                m_PlayerCamera.Add(m_Players[i].m_PlayerNumber, m_Cameras[i]);
                m_Cameras[i].SetTarget(m_Players[i].m_Instance.transform);
            }
        }

        public Vector3 GetAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            // Go through all the targets and add their positions together.
            for (int i = 0; i < m_PTransforms.Length; i++)
            {
                // If the target isn't active, go on to the next one.
                if (!m_PTransforms[i].gameObject.activeSelf)
                    continue;

                // Add to the average and increment the number of targets in the average.
                averagePos += m_PTransforms[i].position;
                numTargets++;
            }

            // If there are targets divide the sum of the positions by the number of them to find the average.
            if (numTargets > 0)
                averagePos /= numTargets;

            // Keep the same y value.
            averagePos.y = transform.position.y;

            // The desired position is the average position;
            return averagePos;
        }

        public float GetRequiredSize(CameraControl targetCamera)
        {
            // Find the position the camera rig is moving towards in its local space.
            Vector3 desiredLocalPos = targetCamera.transform.InverseTransformPoint(GetAveragePosition());

            // Start the camera's size calculation at zero.
            float size = 0f;

            // Go through all the targets...
            for (int i = 0; i < m_PTransforms.Length; i++)
            {
                // ... and if they aren't active continue on to the next target.
                if (!m_PTransforms[i].gameObject.activeSelf)
                    continue;

                // Otherwise, find the position of the target in the camera's local space.
                Vector3 targetLocalPos = targetCamera.transform.InverseTransformPoint(m_PTransforms[i].position);

                // Find the position of the target from the desired position of the camera's local space.
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / targetCamera.GetComponentInChildren<Camera>().aspect);
            }

            // Add the edge buffer to the size.
            size += targetCamera.m_ScreenEdgeBuffer;

            // Make sure the camera's size isn't below the minimum.
            size = Mathf.Max(size, targetCamera.m_MinSize);

            return size;
        }

        /***SETTERS****/
        public void SetPlayers(TankManager[] players) {
            if(players!=null) m_Players = players;
        }
    }
}
