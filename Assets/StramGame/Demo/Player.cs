using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed = 5f;   // �ړ����x
        public float rotationSpeed = 10f; // ��]���x
        public float forceMultiplier = 10f; // �͂̔{��
        private Rigidbody rb;
        [SerializeField] private KeyCode front = KeyCode.W;
        [SerializeField] private KeyCode back = KeyCode.S; // �C��
        [SerializeField] private KeyCode left = KeyCode.A; // �C��
        [SerializeField] private KeyCode right = KeyCode.D; // �C��

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true; // Rigidbody�̉�]���Œ�
        }

        void Update()
        {
            // ���L�[�̓��͂����o
            float horizontal = 0;
            float vertical = 0;

            if (Input.GetKey(left)) horizontal = -1;
            if (Input.GetKey(right)) horizontal = 1;
            if (Input.GetKey(front)) vertical = 1;
            if (Input.GetKey(back)) vertical = -1;

            // ���͂Ɋ�Â��Ĉړ�������ݒ�
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;

            // ���ۂ̈ړ���FixedUpdate�ōs��
            MoveCharacter(movement);

            // ���N���b�N�őO���ɗ͂�������
            if (Input.GetMouseButtonDown(0))
            {
                AddForceForward();
            }
        }

        private void MoveCharacter(Vector3 movement)
        {
            if (movement != Vector3.zero)
            {
                // �ړ������������悤�ɉ�]
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Rigidbody�̈ʒu���ړ�
                rb.MovePosition(rb.position + movement * Time.deltaTime);
            }
        }

        private void AddForceForward()
        {
            // �O���ɗ͂�������
            Vector3 forwardForce = transform.forward * forceMultiplier;
            Vector3 upwardForce = Vector3.up * forceMultiplier; // ������̗�
            rb.AddForce(forwardForce + upwardForce, ForceMode.Impulse); // �O���Ə�����̗͂�������
        }

    }
}
