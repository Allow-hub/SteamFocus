using UnityEngine;

namespace TechC
{
    public class SteamVent : MonoBehaviour
    {
        [Header("���C")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("���C�̐ݒ�")]
        [SerializeField] private float launchForce = 10f; // ������ɔ�΂���
        [SerializeField] private float steamInterval = 2f; // ���˂̃^�C�~���O�Ԋu

        private bool isPlayerInArea = false;
        private Rigidbody playerRb;
        private float timer;

        private void Update()
        {
            // ���˃^�C�~���O���J�E���g
            timer += Time.deltaTime;

            // ��莞�Ԃ��Ƃɕ��˂��s��
            if (timer >= steamInterval)
            {
                // �v���C���[���G���A���ɂ���ꍇ�A��ɔ�΂�
                if (isPlayerInArea && playerRb != null)
                {
                    LaunchPlayer();
                }
                timer = 0f; // �^�C�}�[�����Z�b�g
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                playerRb = other.gameObject.transform.parent.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    isPlayerInArea = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // �v���C���[���G���A����o�����A�t���O�����Z�b�g
            if (other.CompareTag("Ball"))
            {
                isPlayerInArea = false;
                playerRb = null;
            }
        }

        private void LaunchPlayer()
        {
            // �v���C���[��Rigidbody�ɏ�����̗͂�������
            playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
    }
}
