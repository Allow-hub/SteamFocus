using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed = 5f;   // 移動速度
        public float rotationSpeed = 10f; // 回転速度
        public float forceMultiplier = 10f; // 力の倍率
        private Rigidbody rb;
        [SerializeField] private KeyCode front = KeyCode.W;
        [SerializeField] private KeyCode back = KeyCode.S; // 修正
        [SerializeField] private KeyCode left = KeyCode.A; // 修正
        [SerializeField] private KeyCode right = KeyCode.D; // 修正

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true; // Rigidbodyの回転を固定
        }

        void Update()
        {
            // 矢印キーの入力を検出
            float horizontal = 0;
            float vertical = 0;

            if (Input.GetKey(left)) horizontal = -1;
            if (Input.GetKey(right)) horizontal = 1;
            if (Input.GetKey(front)) vertical = 1;
            if (Input.GetKey(back)) vertical = -1;

            // 入力に基づいて移動方向を設定
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;

            // 実際の移動はFixedUpdateで行う
            MoveCharacter(movement);

            // 左クリックで前方に力を加える
            if (Input.GetMouseButtonDown(0))
            {
                AddForceForward();
            }
        }

        private void MoveCharacter(Vector3 movement)
        {
            if (movement != Vector3.zero)
            {
                // 移動方向を向くように回転
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Rigidbodyの位置を移動
                rb.MovePosition(rb.position + movement * Time.deltaTime);
            }
        }

        private void AddForceForward()
        {
            // 前方に力を加える
            Vector3 forwardForce = transform.forward * forceMultiplier;
            Vector3 upwardForce = Vector3.up * forceMultiplier; // 上向きの力
            rb.AddForce(forwardForce + upwardForce, ForceMode.Impulse); // 前方と上向きの力を加える
        }

    }
}
