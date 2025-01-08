using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class AreaLimit : MonoBehaviour
    {
        [SerializeField] private Vector3 minBounds; // 範囲の最小値 (x, y, z)
        [SerializeField] private Vector3 maxBounds; // 範囲の最大値 (x, y, z)
        [SerializeField] private float forceMagnitude = 50f; // 力の大きさ
        [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.5f); // Gizmo の色

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 position = other.transform.position;
                    Vector3 forceDirection = Vector3.zero;

                    if (position.x < minBounds.x)
                        forceDirection.x = minBounds.x - position.x;
                    else if (position.x > maxBounds.x)
                        forceDirection.x = maxBounds.x - position.x;

                    if (position.y < minBounds.y)
                        forceDirection.y = minBounds.y - position.y;
                    else if (position.y > maxBounds.y)
                        forceDirection.y = maxBounds.y - position.y;

                    if (position.z < minBounds.z)
                        forceDirection.z = minBounds.z - position.z;
                    else if (position.z > maxBounds.z)
                        forceDirection.z = maxBounds.z - position.z;

                    if (forceDirection != Vector3.zero)
                    {
                        forceDirection.Normalize();
                        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                    }
                }
            }
        }

        // Gizmo を使った範囲の可視化
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Vector3 center = (minBounds + maxBounds) / 2;
            Vector3 size = maxBounds - minBounds;
            Gizmos.DrawCube(center, size);
        }
    }
}
