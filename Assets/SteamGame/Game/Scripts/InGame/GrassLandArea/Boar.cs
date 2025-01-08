using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Boar : MonoBehaviour
    {
        [SerializeField] private Transform[] movePositions;
        [SerializeField] private float moveSpeed;
        [SerializeField] private  float rotationDuration = 1f;  // 回転にかかる時間


        [SerializeField] private float force;

        [SerializeField] private float upForce;

        private bool isRotating=false;
        private int currentIndex;



        private void Update()
        {
            if (isRotating) return;
            Move();
        }

        private void Move()
        {
            Transform targetPos = movePositions[currentIndex];  // 現在のターゲット位置
            float step = moveSpeed * Time.deltaTime;  // 移動速度に基づく一回のステップ
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);

            if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
            {
                StartCoroutine(RotateToNextPos());  // 目的地に到着したら回転処理を開始
            }
        }

        private IEnumerator RotateToNextPos()
        {
            isRotating = true;

            // 進行方向を計算
            Vector3 direction = (movePositions[currentIndex].position - transform.position).normalized;

            // 現在の回転を保持
            Quaternion currentRotation = transform.rotation;

            // Y軸のみ180度回転させる
            Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - 180f, currentRotation.eulerAngles.z);

            float timeElapsed = 0f;

            // 回転処理
            while (timeElapsed < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, timeElapsed / rotationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;  // 最終的な回転位置を設定

            currentIndex = (currentIndex + 1) % movePositions.Length;  // 次のターゲットに進む

            isRotating = false;
        }



        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb == null) return;

                // 進行方向を計算
                Vector3 direction = (movePositions[currentIndex].position - transform.position).normalized;

                Vector3 additionalUpwardForce = Vector3.up * upForce;  // 上方向に力を加える

                // 合成した力を加える
                rb.AddForce(direction * force + additionalUpwardForce, ForceMode.Impulse);
            }
        }

    }
}
