using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class HairController : MonoBehaviour
    {
        public float power = 40;
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
        private SpringJoint springJoint; // Spring Joint��ێ�����ϐ�

        private bool isLerping = false; // Lerp�����ǂ����̃t���O
        private Vector3 startLerpPosition; // Lerp�J�n���̈ʒu
        private float lerpTime = 0f; // Lerp�̎��Ԍo��
        public float lerpDuration = 1f; // Lerp�ɂ����鎞�ԁi�b�j

        void Start()
        {
            mainCamera = Camera.main; // ���C���J�������擾
            rb = GetComponent<Rigidbody>(); // Rigidbody���擾

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            Move();

            // E�L�[�������ꂽ��Lerp�J�n
            if (Input.GetKeyDown(KeyCode.E))
            {
                isLerping = true;
                startLerpPosition = transform.position; // ���݂̈ʒu��ۑ�
                lerpTime = 0f; // ���Ԍo�߂����Z�b�g
            }

            // Lerp����
            if (isLerping)
            {
                LerpToHead();
            }
        }

        private void LerpToHead()
        {
            // Lerp�̐i�s�x���v�Z
            lerpTime += Time.deltaTime;
            float progress = lerpTime / lerpDuration;

            // ���݈ʒu��Lerp�ōX�V
            transform.position = Vector3.Lerp(startLerpPosition, headObj.transform.position, progress);

            // Lerp������������I��
            if (progress >= 1f)
            {
                isLerping = false; // Lerp���I��
            }
        }

        private void Move()
        {
            // WASD�L�[�ɂ��ړ�
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.A)) moveX = -1f; // ���ړ�
            if (Input.GetKey(KeyCode.D)) moveX = 1f; // �E�ړ�
            if (Input.GetKey(KeyCode.W)) moveZ = 1f; // �O�i
            if (Input.GetKey(KeyCode.S)) moveZ = -1f; // ���

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
                currentJumpForce += jumpIncreaseRate * Time.deltaTime; // ���݂̃W�����v�͂𑝉�
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce); // �ő�W�����v�͂𐧌�
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Force); // ������̗͂�������
            }
            else
            {
                currentJumpForce = constantJumpForce; // ���̗͂ɐݒ�
            }
        }
    }
}
