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
        private SphereCollider headCollider; // 頭のコライダー
        private SpringJoint springJoint; // Spring Jointを保持する変数



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
            //StickToHead();
        }

        private void StickToHead()
        {
            // 粘着処理
            float distanceToHead = Vector3.Distance(transform.position, headObj.transform.position);

            // 頭のコライダーの半径以内にいるかチェック
            if (headCollider != null && distanceToHead < headCollider.radius)
            {
                // 粘着力を加える（Y成分を無視）
                Vector3 directionToHead = (headObj.transform.position - transform.position).normalized;
                directionToHead.y = 0; // Y成分を無視

                // 粘着力を加える
                rb.AddForce(directionToHead * stickForce, ForceMode.Force);
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

        private void OnCollisionEnter(Collision collision)
        {
            // 当たったオブジェクトが特定のタグを持っている場合（例：Hair）
            if (collision.gameObject.CompareTag("Player"))
            {
                CreateSpringJoint(collision.gameObject.transform.GetChild(0).gameObject);
                headCollider = collision.gameObject.GetComponent<SphereCollider>(); // コライダーを取得
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // 当たったオブジェクトが特定のタグを持っていて、Spring Jointが存在する場合
            if (collision.gameObject.CompareTag("Player") && springJoint != null)
            {
                // Spring Jointを無効化
                springJoint.connectedBody = null; // 接続先をnullにする

            }
        }



        public void CreateSpringJoint(GameObject target)
        {
            // Spring Jointが既に存在する場合は、ターゲットを更新する
            if (springJoint != null)
            {
                springJoint.connectedBody = target.GetComponent<Rigidbody>(); // 新しいターゲットのRigidbodyを設定
                return; // 早期リターン
            }

            // Spring Jointを追加
            if (springJoint == null)
                springJoint = gameObject.AddComponent<SpringJoint>();

            // ターゲットのRigidbodyを取得
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                springJoint.connectedBody = targetRb; // 接続先のRigidbodyを設定
            }

            // Spring Jointのプロパティを設定
            springJoint.spring = power; // バネの強さ
            springJoint.damper = 1f; // 減衰
            springJoint.minDistance = 0f; // 最小距離
            springJoint.maxDistance = stickDistance; // 最大距離
        }



    }
}
