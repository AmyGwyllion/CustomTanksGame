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
        protected float m_Size;                                 // The orthographic camera size

        // The class constructor
        public ViewBehaviour(CameraControl cameraControl, Transform target) {
            // The abstract class is from class None by default
            m_Class = E_VIEWCLASS.None;

            // Initialize variables
            m_CameraControl = cameraControl;
            m_Player = target;

            m_MoveVelocity = Vector3.zero;
            m_Size = 12.0f;

            // Get the camera attatched to the camera control and his mask even if they aren't enabled
            m_Camera = cameraControl.GetComponentInChildren<Camera>(true);
            m_Mask = cameraControl.GetComponentInChildren<MaskControl>(true);

        }

        // Virtual functions meant to be overrided by child classes
        protected virtual void Move(float DampTime) { Debug.Log("Reached ViewBehaviour Move()"); }     // Moves the camera to the target position
        protected virtual Vector3 CalculateNewPosition() { return Vector3.zero; }                   // Calculates the new position where to move

        // Returns the class of the child class
        public E_VIEWCLASS GetClass()
        {
            return m_Class;
        }

        // Sets the camera target
        public void SetTarget(Transform target)
        {
            if(target!=null) m_Player = target;
        }

        // We call this method for initializing the camera position without damping time
        public void Initialize()
        {
            // Find the desired position.
            Vector3 target = CalculateNewPosition();

            // Set the camera's position to the desired position without damping.
            m_CameraControl.transform.position = target;

            // Find and set the required size of the camera without damping.
            m_Camera.orthographicSize = m_Size;
        }

        // The base update, the functions are the same name but they will have different behaviours depending on the child class
        public void Update(float DampTime)
        {

            // Move the camera to the target
            Move(DampTime);

        }

        //Check if there are any world bounds in that distance
        protected void CheckBounds(ref Vector3 target)
        {

            // Get the mask the world collider is in
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            // Get the distance between the target and the actual camera rig position
            Vector3 distance = target - m_CameraControl.transform.position;

            // If whe are reaching boundaries from the bottom left or the top right camera view corners
            if (CheckBotLeft(layerMask, distance) || CheckTopRight(layerMask, distance))
            {
                // Flip te X axis for going perpendicular and add that distance to the final camera position
                distance.x = -distance.x;
                target.x += distance.x;
            }

            // If we are reaching boundaries from the top left or bottom right camera view corners
            if (CheckTopLeft(layerMask, distance) || CheckBotRight(layerMask, distance))
            {
                // Flip the Z axis for going perpendicular and add that distance to the final camera position
                distance.z = -distance.z;
                target.z += distance.z;
            }

            //Else if we are actually hitting a bound
            if (CheckBotLeft(layerMask, Vector3.zero))
            {
                Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 0, m_Camera.nearClipPlane));
                
                target.x -= pos.x;
            }

            if (CheckTopRight(layerMask, Vector3.zero))
            {
                Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 1, m_Camera.nearClipPlane));
                
                target.x -= pos.x;
            }


            if (CheckTopLeft(layerMask, Vector3.zero))
            {
                Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 1, m_Camera.nearClipPlane));

                target.z -= pos.z;
            }

            if (CheckBotRight(layerMask, Vector3.zero))
            {
                Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 0, m_Camera.nearClipPlane));

                target.z -= pos.z;
            }

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

            // Else we hit the world boundary collider
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