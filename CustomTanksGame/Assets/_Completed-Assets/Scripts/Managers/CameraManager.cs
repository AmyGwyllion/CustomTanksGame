using System.Collections.Generic;
using UnityEngine;

 /**
  * This script is used to control al cameras in the scene
  */
namespace Complete { 
    public class CameraManager : MonoBehaviour {

        public CameraControl P1_CameraRig;                          // The player one camera
        public CameraControl P2_CameraRig;                          // The player two camera

        public float MaxPlayerDistance;                             // The maximum distance the players can be before whe split the camera view
        private TankManager[] m_Players;                            // All player objects in scene,                                                 populated by GameManager at START()
        private Transform[] m_PTransforms;                          // All player transforms,                                                       populated by GameManager at START()
        private List<CameraControl> m_Cameras;                      // All Cameras Controllers

        private void Awake()
        {
            //Initialize variables
            m_Players = new TankManager[0];
            m_PTransforms = new Transform[0];
            m_Cameras = new List<CameraControl>();
            MaxPlayerDistance = 22.0f;
        }

        void Start ()
        {
            //Get all camera rig childs
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

            // Initialize the player one and player two cameras
            P1_CameraRig.SetStartPositionAndSize();
            P2_CameraRig.SetStartPositionAndSize();
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
            // We are using the main camera as the player one camera too
            // Get the player one and player two cameras
            Camera P1_Camera = P1_CameraRig.GetComponentInChildren<Camera>(true);
            Camera P2_Camera = P2_CameraRig.GetComponentInChildren<Camera>(true);

            // Get the player one and player two targets
            Vector3 P1_Target = P1_CameraRig.GetTarget().position;
            Vector3 P2_Target = P2_CameraRig.GetTarget().position;

            // Calculate the distance between player1 one and player2 
            float dist = (P2_Target - P1_Target).magnitude;

            // Check if the player2 position is inside the player1 camera viewport
            Vector3 coord = P1_Camera.WorldToViewportPoint(P2_Target);
            if ((coord.x > 0.0f && coord.x < 1.0f || coord.y > 0.0f && coord.y < 1.0f) && dist<= MaxPlayerDistance)
            {
                //If player2 is inside player1 field of view change to single view camera
                if (P1_CameraRig.IsViewSplit())
                {
                    P1_CameraRig.ChangeToSingleView();

                    //Disable the second player camera
                    P2_Camera.enabled = false;
                    //P2_CameraRig.ChangeToSingleView();
                }
            }

            //Else if distance is to high change to split view
            else if(dist> MaxPlayerDistance)
            {
                if (!P1_CameraRig.IsViewSplit())
                {
                    P1_CameraRig.ChangeToSplitView();
                    
                    //Enable player two camera and set it tosplit view
                    //P2_CameraRig.ChangeToSplitView();
                    P2_Camera.enabled = true;
                }
            }
           
        }

        // I leaved this methods here so the cameras have no need of getting all the other cameras info too
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

    }
}
