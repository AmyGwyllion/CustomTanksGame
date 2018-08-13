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


        // The class constructor
        public ViewBehaviour(CameraControl cameraControl, Transform target) {
            // The abstract class is from class None by default
            m_Class = E_VIEWCLASS.None;

            // Initialize variables
            m_CameraControl = cameraControl;
            m_Player = target;

            m_MoveVelocity = Vector3.zero;
            m_ZoomSpeed = 0.0f;

            // Get the camera attatched to the camera control and his mask (if exist)
            m_Camera = cameraControl.GetComponentInChildren<Camera>();
            m_Mask = cameraControl.GetComponentInChildren<MaskControl>(true);
        }

        // Returns the class of the child class
        public E_VIEWCLASS GetViewClass()
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
        /*
        protected bool CheckIfClamping(Vector3 newPosition) {
            bool toRet = false;

            toRet = ClampTopLeft(newPosition);

            return toRet;
            //return ClampBotLeft(newPosition) || ClampTopLeft(newPosition) || ClampBotRight(newPosition) || ClampTopRight(newPosition);
        }

        private Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            Vector3 toRet = Vector3.zero;

            Vector3 headsTo = from - to;
            float lenght = headsTo.magnitude;
            //Vector3 direction = headsTo / lenght;

            return toRet;
        }

        protected bool ClampBotLeft(Vector3 newPosition)
        {
            bool toRet = false;
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");
            RaycastHit hit;
            Ray rayBotLeft = m_Camera.ViewportPointToRay(new Vector3(0.0f, 0.0f, 0.0f));
            if (!Physics.Raycast(rayBotLeft, out hit, float.PositiveInfinity, layerMask))
            {
                Vector3 clampPoint = m_Camera.ViewportToWorldPoint(hit.point); 
                if (GetDirection(m_Player.position, newPosition) == GetDirection(m_Player.position, clampPoint)) toRet = true;
                Debug.Log("Clamp Bot Left at" + clampPoint);
            }
            return toRet;
        }

        public bool ClampTopLeft(Vector3 newPosition)
        {
            bool toRet = false;
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");
            RaycastHit hit;
            Ray rayTopLeft = m_Camera.ViewportPointToRay(new Vector3(0.0f, 1.0f, 0.0f));
            if (!Physics.Raycast(rayTopLeft, out hit, float.PositiveInfinity, layerMask))
            {
                Vector3 clampPoint = m_Camera.ViewportToWorldPoint(hit.point);
                //if (GetDirection(m_Player.position, newPosition) == GetDirection(m_Player.position, clampPoint)) toRet = true;
                //Debug.Log("Clamp Top Left at" + clampPoint);

                float pDir = 0.0f;
                Vector3 from = new Vector3(m_Player.transform.forward.x, 0, m_Player.transform.forward.z);
                //Vector3 to = new Vector3(clampPoint.x, 0, clampPoint.z);
                pDir = Vector3.Angle(from, clampPoint);

                Debug.Log("Player direction" + pDir);
            }
            return toRet;
        }

        public bool ClampBotRight(Vector3 newPosition)
        {
            bool toRet = false;
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");
            RaycastHit hit;
            Ray rayBotRight = m_Camera.ViewportPointToRay(new Vector3(1.0f, 0.0f, 0.0f));
            if (!Physics.Raycast(rayBotRight, out hit, float.PositiveInfinity, layerMask))
            {
                Vector3 clampPoint = m_Camera.ViewportToWorldPoint(hit.point);
                if (GetDirection(m_Player.position, newPosition) == GetDirection(m_Player.position, clampPoint)) toRet = true;
                Debug.Log("Clamp Bot Right at" + clampPoint);
            }
            return toRet;
        }

        public bool ClampTopRight(Vector3 newPosition)
        {
            bool toRet = false;
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");
            RaycastHit hit;
            Ray rayTopRight = m_Camera.ViewportPointToRay(new Vector3(1.0f, 1.0f, 0.0f));
            if (!Physics.Raycast(rayTopRight, out hit, float.PositiveInfinity, layerMask))
            {
                Vector3 clampPoint = m_Camera.ViewportToWorldPoint(hit.point);
                if (GetDirection(m_Player.position, newPosition) == GetDirection(m_Player.position, clampPoint)) toRet = true;
                Debug.Log("Clamp Top Right at" + clampPoint);
            }
            return toRet;
        }


        protected Vector3 GetClampNewPosition(Vector3 position)
        {
            /*
            Debug.Log("clamp position: " + m_ClampWorldPoint);
            Debug.Log("initial position: " + position);
            if (m_Clamp)
            {
                float cXPos = Mathf.Abs(m_ClampWorldPoint.x);
                float cZPos = Mathf.Abs(m_ClampWorldPoint.z);

                if (position.x > cXPos) position.x = m_ClampWorldPoint.x;
                if (position.z > cZPos) position.z = m_ClampWorldPoint.z;
            }
            Debug.Log("final position: " + position);
            
            return position;
        }*/

    }
}