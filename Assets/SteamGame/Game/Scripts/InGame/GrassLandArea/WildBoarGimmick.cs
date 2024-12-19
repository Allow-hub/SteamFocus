using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class WildBoarGimmick : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 10.0f; // 最大速度
        [SerializeField] private float minSpeed = 1.0f; // 最小速度
        [SerializeField] private float accelerated = 2.0f; // 加速
        [SerializeField] private float decelerationRate = 5.0f; // 減速
        [SerializeField] private float decelerationDistance = 2.0f; // 減速開始距離
        [SerializeField] private Transform[] points; // 目的地の設定
        [SerializeField] private float waitTime = 1.0f; // 次の目的地まで待つ時間
        [SerializeField] private GameObject Ball;

        private int currentPointIndex = 0; //現在の目的地のインデックス
        private bool isWaiting = false; // 停止中かどうか
        private float currentSpeed = 0.0f; // 現在速度

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            rb.isKinematic = true;
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
            // 現在の目的地までの距離を確認
            Transform targetPoint = points[currentPointIndex];
            Vector3 direction = targetPoint.position - transform.position;
            float distance = direction.magnitude;

            float dynamicDecelerationDistance = Mathf.Clamp(currentSpeed / decelerationDistance, 1.0f, decelerationDistance);

            // 加速または減速
            if (distance > dynamicDecelerationDistance)
            {
                // 加速
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, accelerated * Time.deltaTime);
                // Debug.Log(currentSpeed);
            }
            else if (distance <= dynamicDecelerationDistance && distance > 0.1f)
            {                
                // 減速
                currentSpeed = Mathf.Lerp(currentSpeed, 0, decelerationRate * Time.deltaTime);
                // Debug.Log(currentSpeed);
            }
            else
            {
                // 停止処理
                currentSpeed = 0;
                StartCoroutine(WaitBeforeMoving());
            }

            currentSpeed = Mathf.Max(currentSpeed, minSpeed);

            // 移動処理
           Vector3 movePosition = transform.position + direction.normalized * currentSpeed * Time.deltaTime;
            rb.MovePosition(movePosition);

            // 体の向き
            RotateTowards(direction);
        }

        private IEnumerator WaitBeforeMoving()
        {
            isWaiting = true; // 停止フラグ
            currentSpeed = 0; // 速度をリセット
            yield return new WaitForSeconds(waitTime);
            isWaiting = false; // 停止フラグを解除
            currentPointIndex = (currentPointIndex + 1) % points.Length; // 次の目的地に切り替え

        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                // Y軸方向のみで回転を計算
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(0, -90, 0);
                // 回転を現在の回転に向けてスムーズに補間
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody ballRigdbody = collision.gameObject.GetComponent<Rigidbody>();

                if (ballRigdbody != null)
                {
                    Vector3 targeting = (Ball.transform.position - this.transform.position).normalized;

                    float forceStrength = 900f;
                    ballRigdbody.AddForce(targeting * forceStrength);

                    // Debug.Log("衝突した");
                }
            }

            
        }
    }
}
