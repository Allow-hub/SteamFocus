using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Human : MonoBehaviour
    {
        public float moveSpeed = 5f;   // 移動速度
        public float rotationSpeed = 10f; // 回転速度
        private Rigidbody rb;

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

            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1;
            if (Input.GetKey(KeyCode.UpArrow)) vertical = 1;
            if (Input.GetKey(KeyCode.DownArrow)) vertical = -1;

            // 入力に基づいて移動方向を設定
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;

            // 実際の移動はFixedUpdateで行う
            MoveCharacter(movement);
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
    }
}
