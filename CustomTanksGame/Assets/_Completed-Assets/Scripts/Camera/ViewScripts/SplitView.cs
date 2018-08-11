using UnityEngine;
namespace Complete
{
    public class SplitView : ViewBehaviour
    {
        public SplitView(GameObject parent) : base(parent) { m_Parent = parent; }

        public override void FixedUpdate()
        {
            // Move the camera towards a desired position.
            Move();

            // Change the size of the camera based.
            Zoom();

            UpdateMask();
        }

        protected override void Move() { }
        protected override void Zoom() { }

        private void UpdateMask()
        {

        }
    }
}