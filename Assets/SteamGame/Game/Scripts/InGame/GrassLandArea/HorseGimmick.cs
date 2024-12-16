using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class HorseGimmick : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private Transform[] points;
        [SerializeField] private float waitTime = 1.0f;
        [SerializeField] private GameObject Ball;

        private int currentpointIndex = 0;
        private bool isWaiting = false;

        private float originalSpeed;
        private Rigidbody rb; // Rigidbodyの参照

        private void Start()
        {
            rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
            originalSpeed = speed;
        }

        void Update()
        {
            if (!isWaiting)
            {
                Move();
            }
        }

        private void Move()
        {
            Transform targetPoint = points[currentpointIndex];
            Vector3 direction = targetPoint.position - transform.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                // Rigidbodyを使って移動
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                rb.MovePosition(transform.position + movement);
            }
            else
            {
                StartCoroutine(WaitBeforeMoving());
            }

            RotateTowards(direction);
        }

        private IEnumerator WaitBeforeMoving()
        {
            isWaiting = true;
            speed = 0;
            yield return new WaitForSeconds(waitTime);

            isWaiting = false;
            currentpointIndex = (currentpointIndex + 1) % points.Length;
            speed = originalSpeed;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

                Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(0, 90, 0);

                rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, 360 * Time.deltaTime);
            }
        }
    }
}
