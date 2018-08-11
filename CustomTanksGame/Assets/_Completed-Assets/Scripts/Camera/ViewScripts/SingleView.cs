using UnityEngine;
namespace Complete
{
    public class SingleView : ViewBehaviour
    {
        private Vector3 m_AveragePosition;
        public SingleView(GameObject parent) : base(parent) { m_Parent = parent; }

        public override void FixedUpdate()
        {
            // Move the camera towards a desired position.
            Move();

            // Change the size of the camera based.
            Zoom();
        }

        protected override void Move()
        {
            // Find the average position of the targets.
            m_AveragePosition = m_Parent.GetComponent<CameraManager>().GetAveragePosition();

            // Smoothly transition to that position.
            //transform.position = Vector3.SmoothDamp(transform.position, m_AveragePosition, ref m_MoveVelocity, m_DampTime);
        }

        protected override void Zoom() { }
    }
}
