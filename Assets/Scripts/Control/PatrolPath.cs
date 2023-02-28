using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float wayPointGizmosRadius = 0.3f;
        public int WayPointCount { get; private set; }
        private void Awake() {
            WayPointCount = transform.childCount;
        }
        private void OnDrawGizmos()
        {

            Gizmos.color = Color.yellow;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(transform.GetChild(i).position, wayPointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            return i == transform.childCount - 1 ? 0 : (i + 1);
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
