using System.Collections;
using UnityEngine;

namespace TechC
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 2.0f;
        [SerializeField] private Transform player;
        [SerializeField] private float distance = 5.0f;
        [SerializeField] private float height = 2.0f;
        [SerializeField] private float shakeDuration = 0.5f; // �V�F�C�N�̎���
        [SerializeField] private float shakeMagnitude = 0.3f; // �V�F�C�N�̋��x
        [SerializeField] private float dampingSpeed = 1.0f; // �����X�s�[�h

        private Camera cam;
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private const float minYAngle = -90.0f;
        private const float maxYAngle = 90.0f;
        private Vector3 shakeOffset = Vector3.zero;
        private float initialShakeMagnitude;

        private void Awake()
        {
            cam = Camera.main;
            initialShakeMagnitude = shakeMagnitude; // �����̃V�F�C�N���x��ۑ�
        }

        private void Update()
        {
            // �}�E�X�̓������擾
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // �㉺��]���X�V
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);

            // ���E��]���X�V
            rotationY += mouseX;

            // �v���C���[�̈ʒu����ɃJ�����̈ʒu���v�Z
            Vector3 offset = new Vector3(0, height, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            cam.transform.position = player.position + rotation * offset + shakeOffset;

            // �J�����̉�]���X�V
            cam.transform.LookAt(player.position + Vector3.up * height);
        }

        public void TriggerShake()
        {
            StopAllCoroutines(); // �����̃V�F�C�N������Β�~
            StartCoroutine(Shake());
        }

        private IEnumerator Shake()
        {
            float elapsed = 0.0f;
            shakeMagnitude = initialShakeMagnitude; // �V�F�C�N���x��������

            while (elapsed < shakeDuration)
            {
                // ���X�ɃV�F�C�N�̋��x������������
                float damper = 1.0f - (elapsed / shakeDuration);

                // Perlin�m�C�Y���g�����X���[�Y�ȗh��
                float shakeX = (Mathf.PerlinNoise(Time.time * dampingSpeed, 0) - 0.5f) * 2 * shakeMagnitude * damper;
                float shakeY = (Mathf.PerlinNoise(0, Time.time * dampingSpeed) - 0.5f) * 2 * shakeMagnitude * damper;
                shakeOffset = new Vector3(shakeX, shakeY, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            shakeOffset = Vector3.zero; // �V�F�C�N�I�����ɃI�t�Z�b�g�����Z�b�g
        }
    }
}
