using System.Collections;
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
        private bool isBallOnSeesaw = false;
        private float elapsedTme = 0;

        private void Awake()
        {
            // �����̉�]��ۑ�
            initialRotation = transform.rotation;
        }


        private void Update()
        {
            if (isBallOnSeesaw) return;
            elapsedTme += Time.deltaTime;
            if(elapsedTme > resetDelay)
            {
                StartCoroutine(ResetRotation());
                elapsedTme = 0;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            elapsedTme = 0;
            isBallOnSeesaw = true; // �{�[�����V�[�\�[�ɏ����
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
                isBallOnSeesaw = false; // �{�[�������ꂽ
            }
        }

        private IEnumerator ResetRotationAfterDelay()
        {
            // �w�肵�����ԑҋ@
            yield return new WaitForSeconds(resetDelay);

            // �{�[�����V�[�\�[���痣�ꂽ�ꍇ�̂݃��Z�b�g���J�n
            if (!isBallOnSeesaw)
            {
                // �X����߂�
                StartCoroutine(ResetRotation());
            }
        }

        private IEnumerator ResetRotation()
        {
            // �V�[�\�[�̉�]�������ʒu�ɖ߂�܂ŃX���[�Y�ɕ��
            while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * smoothSpeed);
                yield return null;
            }
            transform.rotation = initialRotation; // �ŏI�I�ɐ��m�ɏ�����]�ɖ߂�
        }
    }
}
