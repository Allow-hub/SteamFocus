using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// チュートリアルエリアの水の浮力の設定
    /// </summary>
    public class WaterBuoyancy : MonoBehaviour
    {
        [SerializeField] private float buoyancy = 1f;  // 浮力の強さ

        // 水面の高さを管理するための変数（必要に応じて調整）
        [SerializeField] private float waterSurfaceHeight = 0f;  // 水面の高さ（必要に応じて調整）

        // Gizmoを表示するかどうかのフラグ
        [SerializeField] private bool showWaterSurfaceGizmo = true;  // Gizmoの表示・非表示を切り替え

        private void OnTriggerEnter(Collider other)
        {
            // "Ball" タグがついているオブジェクトに浮力を適用
            if (other.gameObject.CompareTag("Ball"))
            {
                // ボールに Rigidbody がアタッチされているか確認
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 浮力を適用する
                    ApplyBuoyancy(rb, other.transform);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // 水に浮いている間は浮力を継続的に加える
            if (other.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log("A");
                    ApplyBuoyancy(rb, other.transform);
                }
            }
        }

        private void ApplyBuoyancy(Rigidbody rb, Transform ballTransform)
        {
            // ボールが水面より上か下かによって浮力を計算
            float distanceToSurface = ballTransform.position.y - waterSurfaceHeight;

            // 浮力を計算
            if (distanceToSurface < 0f)
            {
                // 水面より下に沈んでいる場合、浮力を加える
                float buoyantForce = -buoyancy * distanceToSurface;

                // 浮力を加える
                rb.AddForce(Vector3.up * buoyantForce, ForceMode.Force);
            }
        }

        // Gizmoを表示するためのメソッド
        private void OnDrawGizmos()
        {
            // 水面Gizmoの表示・非表示を切り替える
            if (showWaterSurfaceGizmo)
            {
                // 水面の位置に赤い線を描画
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(-10f, waterSurfaceHeight, 0f), new Vector3(10f, waterSurfaceHeight, 0f));

                // 水面を示す球体（オプション）
                Gizmos.DrawSphere(new Vector3(0f, waterSurfaceHeight, 0f), 0.1f);
            }
        }
    }
}
