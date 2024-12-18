using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class SeeSaw : MonoBehaviour
    {
        [Header("シーソー")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("シーソーの傾き設定")]
        [SerializeField] private float tiltAmount = 10f; // 最大傾きの角度
        [SerializeField] private float smoothSpeed = 2f; // 傾きのスムーズさ
        [SerializeField] private float resetDelay = 2f; // 傾きを戻すまでの待機時間（秒）

        private Quaternion initialRotation;

        private void Awake()
        {
            // 初期の回転を保存
            initialRotation = transform.rotation;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            StopCoroutine(ResetRotation());
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;

            // 衝突地点の取得（最初の接触点を使用）
            ContactPoint contact = collision.contacts[0];

            // オブジェクトの中心座標
            Vector3 objectCenter = transform.position;

            // 衝突地点からオブジェクトの中心への方向ベクトルを計算
            Vector3 direction = contact.point - objectCenter;

            // 方向を正規化
            Vector3 normalizedDirection = direction.normalized;

            // ローカル空間に変換（ワールド座標 -> ローカル座標）
            Vector3 localDirection = transform.InverseTransformDirection(normalizedDirection);

            // 傾きを計算 (X軸とZ軸に応じて回転させる)
            float tiltX = localDirection.z * tiltAmount;
            float tiltZ = -localDirection.x * tiltAmount;

            // 新しい目標の回転を設定
            Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ) * initialRotation;

            // 回転をスムーズに補間
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

            // デバッグ表示
            Debug.Log($"方向: {normalizedDirection}, 傾き: ({tiltX}, {tiltZ})");
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                // ボールが離れた後、指定した時間待ってから傾きを戻す
                StartCoroutine(ResetRotationAfterDelay());
            }
        }

        private IEnumerator ResetRotationAfterDelay()
        {
            // 指定した時間待機
            yield return new WaitForSeconds(resetDelay);

            // 傾きを戻す
            StartCoroutine(ResetRotation());
        }

        private IEnumerator ResetRotation()
        {
            while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * smoothSpeed);
                yield return null;
            }
            transform.rotation = initialRotation;
        }
    }
}
