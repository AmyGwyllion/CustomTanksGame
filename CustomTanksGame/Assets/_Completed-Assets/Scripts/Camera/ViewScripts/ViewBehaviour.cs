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

        //Check if there are any world bounds in that direction
        protected bool checkForBounds(Vector3 direction)
        {
            return /*CheckBotLeft(direction) || CheckBotRight(direction) ||*/ CheckTopLeft(direction);//||CheckTopRight(direction);
        }

        // Si en la proxima posicion de la camara encontramos el borde del mundo
        private bool CheckBotLeft(Vector3 direction)
        {
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            //La coordenada de la esquina en el mundo
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 0, m_Camera.nearClipPlane));

            // Le sumamos la direccion a la que se dirige
            pos += direction;

            //Lanzamos un raycast desde esa posicion para ver si colisiona con algo
            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask))
                return true;

            // If we hit the map bounds
            return false;
        }

        // Si en la proxima posicion de la camara encontramos el borde del mundo
        private bool CheckBotRight(Vector3 direction)
        {
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            //La coordenada de la esquina en el mundo
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 0, m_Camera.nearClipPlane));

            // Le sumamos la direccion a la que se dirige
            pos += direction;

            //Lanzamos un raycast desde esa posicion para ver si colisiona con algo
            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask))
                return true;

            // If we hit the map bounds
            return false;
        }

        // Si en la proxima posicion de la camara encontramos el borde del mundo
        private bool CheckTopLeft(Vector3 direction)
        {
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            //La coordenada de la esquina en el mundo
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(0, 1, m_Camera.nearClipPlane));

            // Le sumamos la direccion a la que se dirige
            pos += direction;

            //Lanzamos un raycast desde esa posicion para ver si colisiona con algo
            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask))
                return true;

            // If we hit the map bounds
            return false;
        }

        // Si en la proxima posicion de la camara encontramos el borde del mundo
        private bool CheckTopRight(Vector3 direction)
        {
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            //La coordenada de la esquina en el mundo
            Vector3 pos = m_Camera.ViewportToWorldPoint(new Vector3(1, 1, m_Camera.nearClipPlane));

            // Le sumamos la direccion a la que se dirige
            pos += direction;

            //Lanzamos un raycast desde esa posicion para ver si colisiona con algo
            if (!Physics.Raycast(pos, m_Camera.transform.forward, float.PositiveInfinity, layerMask))
                return true;

            // If we hit the map bounds
            return false;
        }

        /*
        private void Move ()
        {
            // Find the average position of the targets.
            m_AveragePosition = GetComponentInParent<CameraManager>().GetAveragePosition();

            // Smoothly transition to that position.
            //transform.position = Vector3.SmoothDamp(transform.position, m_AveragePosition, ref m_MoveVelocity, m_DampTime);

            //Follow the player
            Vector3 newPosition = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref m_MoveVelocity, m_DampTime);
        }

        private void Zoom ()
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            float requiredSize = GetComponentInParent<CameraManager>().GetRequiredSize(this);
            m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
        }
        */

    }
}