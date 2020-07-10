using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointGizmoRadius = 0.3f;
        [SerializeField] Color waypointColor = Color.red;
        [SerializeField] Color pathColor = Color.yellow;

        private void OnDrawGizmos()
        {
            int numOfChildren = transform.childCount;
            for (int i = 0; i < numOfChildren; i++)
            {
                Vector3 currentChild = GetWaypoint(i);
                Vector3 nextChild = GetWaypoint(GetNextWaypointIndex(i));
                DrawWaypoint(currentChild);
                DrawPath(currentChild, nextChild);
            }
        }

        private void DrawPath(Vector3 currentChild, Vector3 nextChild)
        {
            Gizmos.color = pathColor;
            Gizmos.DrawLine(currentChild, nextChild);
        }

        private void DrawWaypoint(Vector3 currentChild)
        {
            Gizmos.color = waypointColor;
            Gizmos.DrawSphere(currentChild, waypointGizmoRadius);
        }

        public int GetNextWaypointIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

