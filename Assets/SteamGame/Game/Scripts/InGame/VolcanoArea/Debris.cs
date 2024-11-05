using UnityEngine;

namespace TechC
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody rb;

        // 空中で強くする下向きの力の割合
        [SerializeField] private float gravityMultiplier = 5f; // 重力を何倍にするか

        // 地面に近いときの調整（地面との距離が近いときに重力を弱くする）
        [SerializeField] private float groundCheckDistance = 0.1f; // 地面との距離

        // 空中にいるかどうかを示すフラグ
        private bool isHeavy = true; // 初期状態では空中にいると仮定

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();  // Rigidbodyの参照を取得
        }

        private void Update()
        {
            // オブジェクトが空中にいる間は下向きの力を強化する
            if (transform.position.y > groundCheckDistance)
            {
                if (!isHeavy) // 空中にいるが、重力が強化されていない場合
                {
                    isHeavy = true; // 重力強化フラグをオンにする
                }

                // 空中にいる場合、追加の重力を加える
                ApplyDownwardForce();
            }
            else
            {
                if (isHeavy) // 地面に接触した場合、フラグを戻して重力強化を解除
                {
                    isHeavy = false;
                }
            }
        }

        // 空中にいる間に強い下向きの力を加える
        private void ApplyDownwardForce()
        {
            if (rb != null && isHeavy)
            {
                // 現在の重力に基づいて強い下向きの力を追加
                Vector3 downwardForce = Physics.gravity * gravityMultiplier;
                rb.AddForce(downwardForce, ForceMode.Acceleration); // 重力を加える
            }
        }

        // 衝突時の処理（地面にぶつかったときに重力を戻す）
        private void OnCollisionEnter(Collision col)
        {
            // 衝突したとき、オブジェクトが地面に接触したら重力の強化を解除
            if (col.relativeVelocity.magnitude > 0.1f) // 衝突した際の条件
            {
                isHeavy = false; // 地面に接触したらフラグを解除
                Debug.Log("Debris hit the ground, gravity normal."); // デバッグ用
            }
        }

        // 衝突から浮上している場合など、重力を再適用する場合
        private void OnCollisionExit(Collision col)
        {
            if (col.relativeVelocity.magnitude < 0.1f)
            {
                isHeavy = true; // 空中に戻ったら重力を強化
                Debug.Log("Debris is in the air, gravity enhanced.");
            }
        }
    }
}
