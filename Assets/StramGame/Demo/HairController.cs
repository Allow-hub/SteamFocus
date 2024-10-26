using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class HairController : MonoBehaviour
    {
        public float rotationSpeed = 5f; // �I�u�W�F�N�g�̉�]���x
        public float moveSpeed = 5f; // �ړ����x
        public float maxJumpForce = 5f; // �ő�W�����v��
        public float jumpIncreaseRate = 1f; // �W�����v�͂̑�����
        public float constantJumpForce = 2f; // ���̃W�����v��
        public float stickDistance = 1.5f; // �S������
        public float stickForce = 5f; // �S����
        [SerializeField] private GameObject headObj; // �z���Ώۂ̃I�u�W�F�N�g
        private Camera mainCamera; // ���C���J����
        private Rigidbody rb; // Rigidbody�R���|�[�l���g
        private float currentJumpForce = 0f; // ���݂̃W�����v��
        private SphereCollider headCollider; // ���̃R���C�_�[

        void Start()
        {
            mainCamera = Camera.main; // ���C���J�������擾
            rb = GetComponent<Rigidbody>(); // Rigidbody���擾
            headCollider = headObj.GetComponent<SphereCollider>(); // ���̃R���C�_�[���擾
            Cursor.lockState = CursorLockMode.Locked; // �J�[�\������ʒ����Ƀ��b�N
        }

        void Update()
        {
            Move();
            StickToHead();
        }

        private void StickToHead()
        {
            // �S������
            float distanceToHead = Vector3.Distance(transform.position, headObj.transform.position);

            // ���̃R���C�_�[�̔��a�ȓ��ɂ��邩�`�F�b�N
            if (distanceToHead < headCollider.radius)
            {
                // �S���͂�������iY�����𖳎��j
                Vector3 directionToHead = new Vector3(headObj.transform.position.x - transform.position.x, 0, headObj.transform.position.z - transform.position.z).normalized;

                // �S���͂�������
                rb.AddForce(directionToHead * stickForce, ForceMode.Force);
            }
        }

        private void Move()
        {
            // WASD�L�[�ɂ��ړ�
            float moveX = Input.GetAxis("Horizontal"); // A/D�L�[�܂��͍�/�E���
            float moveZ = Input.GetAxis("Vertical"); // W/S�L�[�܂��͏�/�����

            Vector3 movement = new Vector3(moveX, 0, moveZ).normalized; // �ړ������̃x�N�g��
            Vector3 moveDirection = mainCamera.transform.TransformDirection(movement);
            moveDirection.y = 0; // Y�����𖳎�
            moveDirection *= moveSpeed; // �ړ����x��K�p

            // Rigidbody���g���Ĉړ�
            rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

            // �J�����̌����Ɋ�Â��ăI�u�W�F�N�g��Y������]������
            if (movement.magnitude > 0.1f) // �ړ����Ă���ꍇ�̂݉�]
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection); // �ړ�����������
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // �W�����v�����i�����Ă���Ԕ�ԁj
            if (Input.GetKey(KeyCode.Space)) // �X�y�[�X�L�[��������Ă����
            {
                // ���݂̃W�����v�͂𑝉�������
                currentJumpForce += jumpIncreaseRate * Time.deltaTime;

                // �ő�W�����v�͂𒴂��Ȃ��悤�ɐ���
                if (currentJumpForce > maxJumpForce)
                {
                    currentJumpForce = maxJumpForce;
                }

                // ������̗͂�������iY�����̂݁j
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Force);
            }
            else
            {
                // �X�y�[�X�L�[�������ꂽ��A���݂̃W�����v�͂����Z�b�g
                currentJumpForce = constantJumpForce; // ���̗͂ɐݒ�
            }
        }
    }
}
    