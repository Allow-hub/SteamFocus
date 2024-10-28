using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class HairController : MonoBehaviour
    {
        public float power = 40;
        public float rotationSpeed = 5f; // オブジェクトの回転速度
        public float moveSpeed = 5f; // 移動速度
        public float maxJumpForce = 5f; // 最大ジャンプ力
        public float jumpIncreaseRate = 1f; // ジャンプ力の増加率
        public float constantJumpForce = 2f; // 一定のジャンプ力
        public float stickDistance = 1.5f; // 粘着距離
        public float stickForce = 5f; // 粘着力
        [SerializeField] private GameObject headObj; // 吸着対象のオブジェクト
        private Camera mainCamera; // メインカメラ
        private Rigidbody rb; // Rigidbodyコンポーネント
        private float currentJumpForce = 0f; // 現在のジャンプ力
        private SpringJoint springJoint; // Spring Jointを保持する変数

        private bool isLerping = false; // Lerp中かどうかのフラグ
        private Vector3 startLerpPosition; // Lerp開始時の位置
        private float lerpTime = 0f; // Lerpの時間経過
        public float lerpDuration = 1f; // Lerpにかける時間（秒）

        void Start()
        {
            mainCamera = Camera.main; // メインカメラを取得
            rb = GetComponent<Rigidbody>(); // Rigidbodyを取得

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            Move();

            // Eキーが押されたらLerp開始
            if (Input.GetKeyDown(KeyCode.E))
            {
                isLerping = true;
                startLerpPosition = transform.position; // 現在の位置を保存
                lerpTime = 0f; // 時間経過をリセット
            }

            // Lerp処理
            if (isLerping)
            {
                LerpToHead();
            }
        }

        private void LerpToHead()
        {
            // Lerpの進行度を計算
            lerpTime += Time.deltaTime;
            float progress = lerpTime / lerpDuration;

            // 現在位置をLerpで更新
            transform.position = Vector3.Lerp(startLerpPosition, headObj.transform.position, progress);

            // Lerpが完了したら終了
            if (progress >= 1f)
            {
                isLerping = false; // Lerpを終了
            }
        }

        private void Move()
        {
            // WASDキーによる移動
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.A)) moveX = -1f; // 左移動
            if (Input.GetKey(KeyCode.D)) moveX = 1f; // 右移動
            if (Input.GetKey(KeyCode.W)) moveZ = 1f; // 前進
            if (Input.GetKey(KeyCode.S)) moveZ = -1f; // 後退

            Vector3 movement = new Vector3(moveX, 0, moveZ).normalized; // 移動方向のベクトル
            Vector3 moveDirection = mainCamera.transform.TransformDirection(movement);
            moveDirection.y = 0; // Y成分を無視
            moveDirection *= moveSpeed; // 移動速度を適用

            // Rigidbodyを使って移動
            rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

            // カメラの向きに基づいてオブジェクトのY軸を回転させる
            if (movement.magnitude > 0.1f) // 移動している場合のみ回転
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection); // 移動方向を向く
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // ジャンプ処理（押している間飛ぶ）
            if (Input.GetKey(KeyCode.Space)) // スペースキーが押されている間
            {
                currentJumpForce += jumpIncreaseRate * Time.deltaTime; // 現在のジャンプ力を増加
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce); // 最大ジャンプ力を制限
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Force); // 上方向の力を加える
            }
            else
            {
                currentJumpForce = constantJumpForce; // 一定の力に設定
            }
        }
    }
}
