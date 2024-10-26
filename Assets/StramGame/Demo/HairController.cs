using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class HairController : MonoBehaviour
    {
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
        private SphereCollider headCollider; // 頭のコライダー

        void Start()
        {
            mainCamera = Camera.main; // メインカメラを取得
            rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
            headCollider = headObj.GetComponent<SphereCollider>(); // 頭のコライダーを取得
            Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央にロック
        }

        void Update()
        {
            Move();
            StickToHead();
        }

        private void StickToHead()
        {
            // 粘着処理
            float distanceToHead = Vector3.Distance(transform.position, headObj.transform.position);

            // 頭のコライダーの半径以内にいるかチェック
            if (distanceToHead < headCollider.radius)
            {
                // 粘着力を加える（Y成分を無視）
                Vector3 directionToHead = new Vector3(headObj.transform.position.x - transform.position.x, 0, headObj.transform.position.z - transform.position.z).normalized;

                // 粘着力を加える
                rb.AddForce(directionToHead * stickForce, ForceMode.Force);
            }
        }

        private void Move()
        {
            // WASDキーによる移動
            float moveX = Input.GetAxis("Horizontal"); // A/Dキーまたは左/右矢印
            float moveZ = Input.GetAxis("Vertical"); // W/Sキーまたは上/下矢印

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
                // 現在のジャンプ力を増加させる
                currentJumpForce += jumpIncreaseRate * Time.deltaTime;

                // 最大ジャンプ力を超えないように制限
                if (currentJumpForce > maxJumpForce)
                {
                    currentJumpForce = maxJumpForce;
                }

                // 上方向の力を加える（Y成分のみ）
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Force);
            }
            else
            {
                // スペースキーが離されたら、現在のジャンプ力をリセット
                currentJumpForce = constantJumpForce; // 一定の力に設定
            }
        }
    }
}
    