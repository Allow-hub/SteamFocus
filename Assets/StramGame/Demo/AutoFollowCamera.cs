using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class AutoFollowCameraArm : MonoBehaviour
    {
        [SerializeField] private GameObject player; // �v���C���[�I�u�W�F�N�g
        [SerializeField] private float distance = 5f; // �v���C���[�Ƃ̋���
        [SerializeField] private float height = 2f; // �J�����̍���
        [SerializeField] private float smoothSpeed = 0.1f; // �J�����̃X���[�Y��
        [SerializeField] private float rotationSpeed = 100f; // ��]���x
        [SerializeField] private float verticalAngleLimit = 80f; // ������]�̐���

        private float currentRotationY; // ���݂�Y����]
        private float currentRotationX; // ���݂�X����]

        private void Start()
        {
            currentRotationY = player.transform.eulerAngles.y; // ����Y����]��ݒ�
        }

        private void Update()
        {
            // �}�E�X�̈ړ��ʂ��擾
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Y����]���X�V
            currentRotationY += mouseX;

            // X����]���X�V�i�㉺�����j
            currentRotationX -= mouseY;
            currentRotationX = Mathf.Clamp(currentRotationX, -verticalAngleLimit, verticalAngleLimit); // ������K�p
        }

        private void LateUpdate()
        {
            // �v���C���[�̈ʒu����ɃJ�����̈ʒu���v�Z
            Vector3 playerPosition = player.transform.position + Vector3.up * height;
            Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            Vector3 targetPosition = playerPosition - rotation * Vector3.forward * distance;

            // �J�����̈ʒu���X���[�Y�ɍX�V
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.LookAt(playerPosition); // ��Ƀv���C���[������
        }
    }
}
