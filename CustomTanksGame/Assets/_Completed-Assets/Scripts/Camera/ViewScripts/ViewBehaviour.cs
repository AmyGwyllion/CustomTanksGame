using UnityEngine;

/**
 * This is an abstract class used for defining the different camera view modes 
 */
namespace Complete
{

    public abstract class ViewBehaviour
    {
        public enum E_VIEWCLASS {None, Single, Split};          // Enumerator for defining the different camera views inheritances
        protected E_VIEWCLASS m_Class;                          // The class for each inheritances

        protected CameraControl m_CameraControl;                // The CameraControl that is attatched
        protected Camera m_Camera;                              // The camera we want to manipulate
        protected MaskControl m_Mask;                           // Camera Mask (if it has one)

        protected Transform m_Player;                           // The attatched player transform

        protected Vector3 m_MoveVelocity;                       // Reference velocity for the smooth damping of the position
        protected float m_ZoomSpeed;                            // Reference zoom speed for the smooth damping of the zoom
        protected float m_Size;                                 // The actual orthographic camera size
        protected float m_MaxSize;                              // The max camera size value
        protected float m_MinSize;                              // The smallest orthographic size the camera can be.

        // The class constructor
        public ViewBehaviour(CameraControl cameraControl, Transform target) {
            // The abstract class is from class None by default
            m_Class = E_VIEWCLASS.None;

            // Initialize variables
            m_CameraControl = cameraControl;
            m_Player = target;

            m_MoveVelocity = Vector3.zero;
            m_ZoomSpeed = 0.0f;
            m_Size = 12.0f;
            m_MaxSize = 12.0f;
            m_MinSize = 6.5f;

            // Get the camera attatched to the camera control and his mask (if exist)
            m_Camera = cameraControl.GetComponentInChildren<Camera>();
            m_Mask = cameraControl.GetComponentInChildren<MaskControl>(true);
        }

        // Returns the class of the child class
        public E_VIEWCLASS GetClass()
        {
            return m_Class;
        }

        // Virtual functions meant to be overrided by child classes
        public virtual void Initialize() { Debug.Log("Reached ViewBehaviour Initialize"); }         // This method is used to initialize the camera without dumping movement
        public virtual void Move(float DampTime) { Debug.Log("Reached ViewBehaviour Move()"); }     // Moves the camera to the target position
        public virtual void Zoom(float DampTime) { Debug.Log("Reached ViewBehaviour Zoom()"); }     // Zooms the camera to fit the target

        // The base update, the functions are the same name but they will have different behaviours depending on the child class
        public virtual void Update(float DampTime)
        {
            // Move the camera to the child class target
            Move(DampTime);

            // Fit the camera to the child class target
            Zoom(DampTime);
        }

        public void SetTarget(Transform target)
        {
            if(target!=null) m_Player = target;
        }

        protected bool checkZoomBounds()
        {
            //If the actual view has a corner outside the world bounds
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            //BotLeft
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 0, m_Camera.nearClipPlane));
            bool BotLeft = Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask);
            Debug.DrawRay(pos, m_Camera.transform.forward*1000, Color.red);

            //TopRight
            pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 1, m_Camera.nearClipPlane));
            bool TopRight = Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask);
            Debug.DrawRay(pos, m_Camera.transform.forward * 1000, Color.red);

            //BotRight
            m_Camera.ViewportToWorldPoint(new Vector3(1, 0, m_Camera.nearClipPlane));
            bool BotRight = Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask);
            Debug.DrawRay(pos, m_Camera.transform.forward * 1000, Color.red);

            //TopLeft
            pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 1, m_Camera.nearClipPlane));
            bool TopLeft = Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask);
            Debug.DrawRay(pos, m_Camera.transform.forward * 1000, Color.red);

            bool flag = BotLeft && TopRight && BotRight && TopLeft;

            return !flag;
        }

        //Check if there are any world bounds in that distance
        protected Vector3 checkForBounds(ref Vector3 target)
        {
            // Get the mask the world collider is in
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            // Get the distance between the target and the actual camera rig position
            Vector3 distance = target - m_CameraControl.transform.position;

            // If whe are reaching boundaries from the bottom left or the top right camera view corners
            if (CheckBotLeft(layerMask, distance) || CheckTopRight(layerMask, distance))
            {
                
                // Flip te X distance for going perpendicular and add that distance to the final camera position
                distance.x = -distance.x;
                target.x += distance.x;

            }

            // If we are reaching boundaries from the top left or bottom right camera view corners
            if (CheckTopLeft(layerMask, distance) || CheckBotRight(layerMask, distance))
            {
                // Flip the Z distance for going perpendicular and add that distance to the final camera position
                distance.z = -distance.z;
                target.z += distance.z;

            }

            return target;
        }

        // The next functions do the same operations for each camera corner
        private bool CheckBotLeft(int layer, Vector3 distance)
        {
            // Calculate the camera corner point in the world coordinates
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 0, m_Camera.nearClipPlane));

            // Add it the distance we are trying to go
            pos += distance;

            // Do a raycast from the camera to that point, and if we're not hitting nothing means that we have a world boundary
            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layer))
                return true;

            // Else we hitted the world boundary collider
            return false;
        }

        private bool CheckTopRight(int layer, Vector3 distance)
        {
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 1, m_Camera.nearClipPlane));

            pos += distance;

            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layer))
                return true;

            return false;
        }

        private bool CheckBotRight(int layer, Vector3 distance)
        {
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 0, m_Camera.nearClipPlane));

            pos += distance;

            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layer))
                return true;

            return false;
        }

        private bool CheckTopLeft(int layer, Vector3 distance)
        {
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 1, m_Camera.nearClipPlane));

            pos += distance;

            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layer))
                return true;

            return false;
        }

    }
}