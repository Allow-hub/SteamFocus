using System.Collections;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class ChaseCamera : MonoBehaviour
    {
        private Transform player;
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


        [Header("WallCheck")]
        // ���݂̈ʒu
        private Vector3 targetPosition;

        // �ړI�n
        private Vector3 desiredPosition;

        // �ǂ̏Փˏ��
        private RaycastHit wallHit;

        // �ǂɓ��������ʒu
        private Vector3 wallHitPosition;

        // �Փ˂���ǂ̃��C���[
        [SerializeField] private LayerMask wallLayers;

        

        private void Start()
        {
            cam = Camera.main;
            player = GameObject.FindWithTag("Player").gameObject.transform;
            initialShakeMagnitude = shakeMagnitude; // �����̃V�F�C�N���x��ۑ�
        }

        private void Update()
        {
            if (player == null)
                player = GameObject.FindWithTag("Player").gameObject.transform;

            if (player == null) return;

            // �}�E�X�̓������擾
            float mouseX = Input.GetAxis("Mouse X") * GameManager.I.sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * GameManager.I.sensitivity;

            // �㉺��]���X�V
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);

            // ���E��]���X�V
            rotationY += mouseX;

            // �v���C���[�̈ʒu����ɃJ�����̈ʒu���v�Z
            Vector3 offset = new Vector3(0, height, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

            targetPosition = player.position;  // �v���C���[�̌��݈ʒu
            desiredPosition = targetPosition + rotation * offset; // �ړI�n���v�Z

            // �ǃ`�F�b�N
            if (WallCheck())
            {
                // �ǂɏՓ˂��Ă���ꍇ�A�J�����ʒu�𒲐�
                desiredPosition = wallHitPosition + (desiredPosition - targetPosition).normalized * 0.5f; // �Փ˓_�̎�O�ɏ����J������z�u
            }

            // �J�����̈ʒu���X�V
            cam.transform.position = desiredPosition + shakeOffset;

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
        private bool WallCheck()
        {
            if (Physics.Raycast(targetPosition, desiredPosition - targetPosition, out wallHit, Vector3.Distance(targetPosition, desiredPosition), wallLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("A");
                wallHitPosition = wallHit.point; // �ǂɏՓ˂����ʒu��ۑ�
                return true; // �ǂɏՓ˂���
            }
            else
            {
                return false; // �ǂɏՓ˂��Ȃ�����
            }
        }

    }
}
