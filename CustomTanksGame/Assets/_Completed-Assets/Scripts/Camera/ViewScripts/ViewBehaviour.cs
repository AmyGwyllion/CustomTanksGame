using UnityEngine;
namespace Complete
{
    public abstract class ViewBehaviour
    {
        protected GameObject m_Parent;

        public ViewBehaviour(GameObject parent) { m_Parent = parent; }

        public virtual void FixedUpdate() { Debug.Log("Reached ViewBehaviour Update()"); }
        protected virtual void Move() { Debug.Log("Reached ViewBehaviour Move()"); }
        protected virtual void Zoom() { Debug.Log("Reached ViewBehaviour Zoom()"); }

    }
}