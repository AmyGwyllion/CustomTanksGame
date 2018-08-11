using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete { 
    public class CameraManager : MonoBehaviour {

        private TankManager[] m_Players;                        // All player objects in scene
        private Transform[] m_PTransforms;                       // All player transforms
        private Vector3 m_AveragePosition;                      // The average position between all players
        private List<CameraControl> m_Cameras;                  // All Cameras Controllers
        private Dictionary<int, CameraControl> m_PlayerCamera;  // Player asigned camera

        private void Awake()
        {
            //Initialize variables
            m_Players = new TankManager[0];
            m_PTransforms = new Transform[0];
            m_AveragePosition = new Vector3();
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
                m_Cameras[i].SetAllTargets(m_PTransforms);
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

        /***SETTERS****/
        public void SetPlayers(TankManager[] players) {
            if(players!=null) m_Players = players;
        }
    }
}
