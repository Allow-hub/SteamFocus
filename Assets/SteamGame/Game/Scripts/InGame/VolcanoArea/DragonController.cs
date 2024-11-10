using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class DragonController : MonoBehaviour
    {
        [SerializeField] private GameObject wayPointsParent;
        [SerializeField] private float moveSpeed = 5f;
        private Transform[] wayPoints;
        private int currentWaypointIndex = 0;

        private void Awake()
        {
            int childCount = wayPointsParent.transform.childCount;
            wayPoints = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                wayPoints[i] = wayPointsParent.transform.GetChild(i).transform;
            }

            if (wayPoints == null || wayPoints.Length == 0)
                Debug.LogError("WayPoints are not set up correctly.");
        }

        private void Start()
        {
            StartCoroutine(Fly());
        }

        private IEnumerator Fly()
        {
            while (true)
            {
                Transform targetWaypoint = wayPoints[currentWaypointIndex];

                while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;

                yield return null;
            }
        }
    }
}
