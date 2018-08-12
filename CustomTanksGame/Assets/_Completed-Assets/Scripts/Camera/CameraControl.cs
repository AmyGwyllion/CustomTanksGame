using UnityEngine;

namespace Complete
{
    public class CameraControl : MonoBehaviour
    {
        public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
        public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
        public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        public float m_MaxSize = 10.0f;
        public bool m_SplitCamera = false;

        //[NEW]
        private Transform m_Target;
        private GameObject m_MaskPivot;
        private ViewBehaviour m_Behaviour;
        private bool m_isViewSplit;
        private void Awake()
        {
            m_isViewSplit = true;
            m_Behaviour = new SplitView(this, m_Target);
        }

        private void Start()
        {
            ChangeToSingleView();
        }

        public void ChangeToSplitView()
        {
            m_isViewSplit = true;
            m_Behaviour = new SplitView(this, m_Target);
        }

        public void ChangeToSingleView()
        {
            m_isViewSplit = false;
            m_Behaviour = new SingleView(this, m_Target);
        }
        public void LateUpdate()
        {
            checkIfOutOfBounds();
        }

        private void FixedUpdate()
        {
            m_Behaviour.Update(m_DampTime);
        }

        private void checkIfOutOfBounds()
        {
            Camera camera = GetComponentInChildren<Camera>();

            RaycastHit hit;
            Ray rayBotLeft = camera.ViewportPointToRay(new Vector3(0.0f, 0.0f, 0.0f));
            Ray rayBotRight = camera.ViewportPointToRay(new Vector3(1.0f, 0.0f, 0.0f));
            Ray rayTopLeft = camera.ViewportPointToRay(new Vector3(0.0f, 1.0f, 0.0f));
            Ray rayTopRight = camera.ViewportPointToRay(new Vector3(1.0f, 1.0f, 0.0f));

            Debug.DrawRay(rayBotLeft.origin, rayBotLeft.direction * 500, Color.red);
            Debug.DrawRay(rayTopLeft.origin, rayTopLeft.direction * 500, Color.red);
            Debug.DrawRay(rayBotRight.origin, rayBotRight.direction * 500, Color.red);
            Debug.DrawRay(rayTopRight.origin, rayTopRight.direction * 500, Color.red);
            int layerMask = LayerMask.GetMask("WorldRaycastBounds");

            if (Physics.Raycast(rayBotLeft, out hit,  float.PositiveInfinity, layerMask))
            {
                print("rayBotLeft I'm looking at " + hit.collider.name);
            }
            else
                print("rayBotLeft I'm looking at nothing!");

            if (Physics.Raycast(rayTopLeft, out hit, float.PositiveInfinity, layerMask))
            {
                print("rayTopLeft I'm looking at " + hit.collider.name);
            }
            else
                print("rayTopLeft I'm looking at nothing!");


            if (Physics.Raycast(rayBotRight, out hit, float.PositiveInfinity, layerMask))
            {
                print("rayBotRight I'm looking at " + hit.collider.name);
            }
            else
                print("rayBotRight I'm looking at nothing!");


            if (Physics.Raycast(rayTopRight, out hit, float.PositiveInfinity, layerMask))
            {
                print("rayTopRight I'm looking at " + hit.collider.name);
            }
            else
                print("rayTopRight I'm looking at nothing!");


        }

        public void SetTarget(Transform target)
        {
            if (target != null) m_Target = target;
            m_Behaviour.SetTarget(target);
        }

        public Transform GetTarget()
        {
            return m_Target;
        }

        public void SetStartPositionAndSize()
        {
            if (gameObject.activeSelf) m_Behaviour.Initialize(m_DampTime);
        }

        public bool IsViewSplit()
        {
            return m_isViewSplit;
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