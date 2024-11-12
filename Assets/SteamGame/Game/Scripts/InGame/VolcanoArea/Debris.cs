using UnityEngine;

namespace TechC
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody rb;

        //[SerializeField] private float gravityMultiplier = 5f; // 重力を何倍にするか
        //[SerializeField] private float groundCheckDistance = 0.1f; // 地面との距離
        //[SerializeField] private float gravityApplyInterval = 0.5f; // 重力強化の間隔

        //private bool isHeavy = true; // 初期状態では空中にいると仮定
        //private float gravityTimer = 0f; // 重力強化のタイマー

        private Vector3 direction;
        [SerializeField] private Vector2 rotSpeedRange = new Vector2(1, 3);
        private float rotationSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();  // Rigidbodyの参照を取得
        }

        private void OnEnable()
        {
            direction = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            rotationSpeed = Random.Range(rotSpeedRange.x, rotSpeedRange.y);
        }

        private void Update()
        {
            transform.Rotate(direction.x * Time.deltaTime * rotationSpeed, direction.y * Time.deltaTime * rotationSpeed, direction.z * Time.deltaTime * rotationSpeed);

            //// オブジェクトが空中にいる場合の重力制御
            //if (transform.position.y > groundCheckDistance)
            //{
            //    if (!isHeavy)
            //    {
            //        isHeavy = true;
            //    }

            //    // 重力強化のタイマーを進行させる
            //    gravityTimer += Time.deltaTime;
            //    if (gravityTimer >= gravityApplyInterval)
            //    {
            //        // Intervalが経過したら下向きの力を強化する
            //        ApplyDownwardForce();
            //        gravityTimer = 0f; // タイマーをリセット
            //    }
            //}
            //else
            //{
            //    // 地面に接触している場合、重力強化を解除しタイマーをリセット
            //    if (isHeavy)
            //    {
            //        isHeavy = false;
            //        gravityTimer = 0f;
            //    }
            //}
        }

        //// 空中にいる間に強い下向きの力を加える
        //private void ApplyDownwardForce()
        //{
        //    if (rb != null && isHeavy)
        //    {
        //        // 現在の重力に基づいて強い下向きの力を追加
        //        Vector3 downwardForce = Physics.gravity * gravityMultiplier;
        //        rb.AddForce(downwardForce, ForceMode.Acceleration); // 重力を加える
        //    }
        //}

        //// 衝突時の処理（地面にぶつかったときに重力を戻す）
        //private void OnCollisionEnter(Collision col)
        //{
        //    if (col.relativeVelocity.magnitude > 0.1f)
        //    {
        //        isHeavy = false;
        //        Debug.Log("Debris hit the ground, gravity normal.");
        //    }
        //}

        //// 衝突から浮上している場合など、重力を再適用する場合
        //private void OnCollisionExit(Collision col)
        //{
        //    if (col.relativeVelocity.magnitude < 0.1f)
        //    {
        //        isHeavy = true;
        //        Debug.Log("Debris is in the air, gravity enhanced.");
        //    }
        //}
    }
}
