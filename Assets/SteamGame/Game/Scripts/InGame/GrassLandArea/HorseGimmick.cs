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
                // 現在のY位置を保持し、XとZを固定
                Vector3 currentRotation = rb.rotation.eulerAngles;

                // XZ平面上の方向を保つ
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

                // 目標回転を作成（X回転は-90に固定）
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(0, 180, 0);

                // X軸とZ軸の回転を保持し、Y軸のみ更新
                float fixedX = currentRotation.x;
                float fixedZ = currentRotation.z;
                float targetY = targetRotation.eulerAngles.y + 180f;

                // 回転を更新
                Quaternion finalRotation = Quaternion.Euler(fixedX, targetY, fixedZ);

                // 回転のみ変更し、位置には影響を与えないようにする
                rb.MoveRotation(finalRotation);
            }
        }
    }
}
