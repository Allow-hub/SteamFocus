using UnityEngine;

namespace TechC
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody rb;

        // �󒆂ŋ������鉺�����̗͂̊���
        [SerializeField] private float gravityMultiplier = 5f; // �d�͂����{�ɂ��邩

        // �n�ʂɋ߂��Ƃ��̒����i�n�ʂƂ̋������߂��Ƃ��ɏd�͂��キ����j
        [SerializeField] private float groundCheckDistance = 0.1f; // �n�ʂƂ̋���

        // �󒆂ɂ��邩�ǂ����������t���O
        private bool isHeavy = true; // ������Ԃł͋󒆂ɂ���Ɖ���

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();  // Rigidbody�̎Q�Ƃ��擾
        }

        private void Update()
        {
            // �I�u�W�F�N�g���󒆂ɂ���Ԃ͉������̗͂���������
            if (transform.position.y > groundCheckDistance)
            {
                if (!isHeavy) // �󒆂ɂ��邪�A�d�͂���������Ă��Ȃ��ꍇ
                {
                    isHeavy = true; // �d�͋����t���O���I���ɂ���
                }

                // �󒆂ɂ���ꍇ�A�ǉ��̏d�͂�������
                ApplyDownwardForce();
            }
            else
            {
                if (isHeavy) // �n�ʂɐڐG�����ꍇ�A�t���O��߂��ďd�͋���������
                {
                    isHeavy = false;
                }
            }
        }

        // �󒆂ɂ���Ԃɋ����������̗͂�������
        private void ApplyDownwardForce()
        {
            if (rb != null && isHeavy)
            {
                // ���݂̏d�͂Ɋ�Â��ċ����������̗͂�ǉ�
                Vector3 downwardForce = Physics.gravity * gravityMultiplier;
                rb.AddForce(downwardForce, ForceMode.Acceleration); // �d�͂�������
            }
        }

        // �Փˎ��̏����i�n�ʂɂԂ������Ƃ��ɏd�͂�߂��j
        private void OnCollisionEnter(Collision col)
        {
            // �Փ˂����Ƃ��A�I�u�W�F�N�g���n�ʂɐڐG������d�͂̋���������
            if (col.relativeVelocity.magnitude > 0.1f) // �Փ˂����ۂ̏���
            {
                isHeavy = false; // �n�ʂɐڐG������t���O������
                Debug.Log("Debris hit the ground, gravity normal."); // �f�o�b�O�p
            }
        }

        // �Փ˂��畂�サ�Ă���ꍇ�ȂǁA�d�͂��ēK�p����ꍇ
        private void OnCollisionExit(Collision col)
        {
            if (col.relativeVelocity.magnitude < 0.1f)
            {
                isHeavy = true; // �󒆂ɖ߂�����d�͂�����
                Debug.Log("Debris is in the air, gravity enhanced.");
            }
        }
    }
}
