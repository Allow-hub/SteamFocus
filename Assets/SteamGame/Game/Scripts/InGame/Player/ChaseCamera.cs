using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 2.0f;
        [SerializeField] private Transform player; // �v���C���[��Transform
        [SerializeField] private float distance = 5.0f; // �v���C���[����̋���
        [SerializeField] private float height = 2.0f; // �J�����̍���
        private Camera cam;
        private float rotationX = 0.0f; // �㉺��]��ێ�����ϐ�
        private float rotationY = 0.0f; // ���E��]��ێ�����ϐ�
        private const float minYAngle = -90.0f; // �ŏ��p�x
        private const float maxYAngle = 90.0f;  // �ő�p�x

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            // �}�E�X�̓������擾
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // �㉺��]���X�V
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle); // 90�x�̐�����K�p

            // ���E��]���X�V
            rotationY += mouseX;

            // �v���C���[�̈ʒu����ɃJ�����̈ʒu���v�Z
            Vector3 offset = new Vector3(0, height, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            cam.transform.position = player.position + rotation * offset;

            // �J�����̉�]���X�V
            cam.transform.LookAt(player.position + Vector3.up * height); // �v���C���[�̈ʒu�����߂�
        }
    }
}
