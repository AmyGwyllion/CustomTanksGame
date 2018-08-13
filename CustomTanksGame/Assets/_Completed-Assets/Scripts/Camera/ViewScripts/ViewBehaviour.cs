using UnityEngine;
namespace Complete
{
    public abstract class ViewBehaviour
    {
        public enum E_VIEWCLASS {None, Single, Split};

        protected E_VIEWCLASS m_Class;
        protected Vector3 m_TargetPosition;
        protected CameraControl m_CameraControl;    // The CameraControl that is attatched
        protected Vector3 m_MoveVelocity;           // Reference velocity for the smooth damping of the position
        protected float m_ZoomSpeed;                // Reference zoom speed for the smooth damping of the zoom
        protected Camera m_Camera;                  // The camera we want to manipulate
        protected Transform m_Target;               // Camera player target
        protected MaskControl m_Mask;               // Camera Mask
        protected bool m_Clamp;                       // If view is going out bounds
        protected Vector3 m_ClampWorldPoint;          // World point


        public ViewBehaviour(CameraControl cameraControl, Transform target) {
            m_Class = E_VIEWCLASS.None;
            m_TargetPosition = Vector3.zero;
            m_CameraControl = cameraControl;
            m_MoveVelocity = Vector3.zero;
            m_ZoomSpeed = 0.0f;
            m_Camera = cameraControl.GetComponentInChildren<Camera>();
            m_Target = target;
            m_Mask = cameraControl.GetComponentInChildren<MaskControl>(true);
            m_Clamp = false;
            m_ClampWorldPoint = Vector3.zero;
        }

        public virtual void Initialize(float DampTime) { Debug.Log("Reached ViewBehaviour Initialize"); }
        public virtual void Move(float DampTime) { Debug.Log("Reached ViewBehaviour Move()"); }
        public virtual void Zoom(float DampTime) { Debug.Log("Reached ViewBehaviour Zoom()"); }

        public virtual void Update(float DampTime)
        {
            Move(DampTime);
            Zoom(DampTime);
        }

        public void SetTarget(Transform target)
        {
            if(target!=null) m_Target = target;
        }

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
                if (GetDirection(m_Target.position, newPosition) == GetDirection(m_Target.position, clampPoint)) toRet = true;
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
                //if (GetDirection(m_Target.position, newPosition) == GetDirection(m_Target.position, clampPoint)) toRet = true;
                //Debug.Log("Clamp Top Left at" + clampPoint);

                float pDir = 0.0f;
                Vector3 from = new Vector3(m_Target.transform.forward.x, 0, m_Target.transform.forward.z);
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
                if (GetDirection(m_Target.position, newPosition) == GetDirection(m_Target.position, clampPoint)) toRet = true;
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
                if (GetDirection(m_Target.position, newPosition) == GetDirection(m_Target.position, clampPoint)) toRet = true;
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
            */
            return position;
        }

        public E_VIEWCLASS GetViewClass() {
            return m_Class; }
    }
}