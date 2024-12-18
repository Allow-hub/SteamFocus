using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class SeeSaw : MonoBehaviour
    {
        [Header("�V�[�\�[")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("�V�[�\�[�̌X���ݒ�")]
        [SerializeField] private float tiltAmount = 10f; // �ő�X���̊p�x
        [SerializeField] private float smoothSpeed = 2f; // �X���̃X���[�Y��
        [SerializeField] private float resetDelay = 2f; // �X����߂��܂ł̑ҋ@���ԁi�b�j

        private Quaternion initialRotation;

        private void Awake()
        {
            // �����̉�]��ۑ�
            initialRotation = transform.rotation;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            StopCoroutine(ResetRotation());
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;

            // �Փ˒n�_�̎擾�i�ŏ��̐ڐG�_���g�p�j
            ContactPoint contact = collision.contacts[0];

            // �I�u�W�F�N�g�̒��S���W
            Vector3 objectCenter = transform.position;

            // �Փ˒n�_����I�u�W�F�N�g�̒��S�ւ̕����x�N�g�����v�Z
            Vector3 direction = contact.point - objectCenter;

            // �����𐳋K��
            Vector3 normalizedDirection = direction.normalized;

            // ���[�J����Ԃɕϊ��i���[���h���W -> ���[�J�����W�j
            Vector3 localDirection = transform.InverseTransformDirection(normalizedDirection);

            // �X�����v�Z (X����Z���ɉ����ĉ�]������)
            float tiltX = localDirection.z * tiltAmount;
            float tiltZ = -localDirection.x * tiltAmount;

            // �V�����ڕW�̉�]��ݒ�
            Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ) * initialRotation;

            // ��]���X���[�Y�ɕ��
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

            // �f�o�b�O�\��
            Debug.Log($"����: {normalizedDirection}, �X��: ({tiltX}, {tiltZ})");
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                // �{�[�������ꂽ��A�w�肵�����ԑ҂��Ă���X����߂�
                StartCoroutine(ResetRotationAfterDelay());
            }
        }

        private IEnumerator ResetRotationAfterDelay()
        {
            // �w�肵�����ԑҋ@
            yield return new WaitForSeconds(resetDelay);

            // �X����߂�
            StartCoroutine(ResetRotation());
        }

        private IEnumerator ResetRotation()
        {
            while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * smoothSpeed);
                yield return null;
            }
            transform.rotation = initialRotation;
        }
    }
}
