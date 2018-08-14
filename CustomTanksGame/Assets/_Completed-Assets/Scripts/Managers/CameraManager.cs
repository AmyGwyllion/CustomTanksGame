﻿using System.Collections.Generic;
using UnityEngine;

 /**
  * This script is used to controll al cameras in the scene
  */
namespace Complete { 
    public class CameraManager : MonoBehaviour {

        public float MaxPlayerDistance;                             // The maximum distance the players can be before whe split the camera view
        private TankManager[] m_Players;                            // All player objects in scene,                                                 populated by GameManager at START()
        private Transform[] m_PTransforms;                          // All player transforms,                                                       populated by GameManager at START()
        private List<CameraControl> m_Cameras;                      // All Cameras Controllers
        private Dictionary<int, CameraControl> m_PlayerCamera;      // All cameras by player number

        private void Awake()
        {
            //Initialize variables
            m_Players = new TankManager[0];
            m_PTransforms = new Transform[0];
            m_Cameras = new List<CameraControl>();
            m_PlayerCamera = new Dictionary<int, CameraControl>();
            MaxPlayerDistance = 20.0f;
        }

        void Start ()
        {
            //Get all camera childs
            foreach (Transform child in transform) m_Cameras.Add(child.gameObject.GetComponent<CameraControl>());
        }
         
        // This function is called by the RoundStarting() coroutine, in GameLoop(), at GameManager START() function
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

        // This function is called by the GameManager for populating the players array at his START() function
        public void SetPlayers(TankManager[] players) {
            if(players!=null) m_Players = players;
        }

        // This function is called by the GameManager for linkinkg cameras to players at his START() function
        public void SetAllCameraTargets()
        {
            for (int i = 0; i < m_Players.Length && i< m_Cameras.Count; i++)
            {
                m_PlayerCamera.Add(m_Players[i].m_PlayerNumber, m_Cameras[i]);
                m_Cameras[i].SetTarget(m_Players[i].m_Instance.transform);
            }
        }

        private void FixedUpdate()
        {
            // Only player one and player two cameras are going to be cheched for splitting view
            CheckP1P2Cameras();
        }

        //If their camera bounds are close merge view
        private void CheckP1P2Cameras()
        {
            // Get the player one camera
            Camera P1 = m_PlayerCamera[1].GetComponentInChildren<Camera>();

            // Calculate the distance between player1 one and player2 
            float dist = Vector3.Distance(m_PTransforms[0].position, m_PTransforms[1].position);

            // Check if the player2 position is inside the player1 camera viewport
            Vector3 coord = P1.WorldToViewportPoint(m_PTransforms[1].position);
            if ((coord.x > 0.0f && coord.x < 1.0f || coord.y > 0.0f && coord.y < 1.0f) && dist<= MaxPlayerDistance)
            {
                //If player2 is inside player1 field of view change to single view camera
                if (m_PlayerCamera[1].IsViewSplit())
                {
                    m_PlayerCamera[1].ChangeToSingleView();
                    m_PlayerCamera[2].ChangeToSingleView();
                }
            }
            //Else if distance is to high change to split view
            else if(dist> MaxPlayerDistance)
            {
                if (!m_PlayerCamera[1].IsViewSplit())
                {
                    m_PlayerCamera[1].ChangeToSplitView();
                    m_PlayerCamera[2].ChangeToSplitView();
                }
            }
           
        }

        // This method is used from ViewBehaviours scripts
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

        // This method is used from ViewBehaviours scripts
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

    }
}
