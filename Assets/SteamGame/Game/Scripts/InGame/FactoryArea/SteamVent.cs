using UnityEngine;
using System.Collections;  // �R���[�`���p�ɒǉ�

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
        [SerializeField] private float smokeDuration = 2f; // ���C�G�t�F�N�g�̕\������
        [SerializeField] private GameObject smokeEffect;

        private bool isPlaying = false;

        private bool isPlayerInArea = false;
        private Rigidbody playerRb;
        private float timer;

        private void Awake()
        {
            smokeEffect.SetActive(false);
        }

        private void Update()
        {
            // ���˃^�C�~���O���J�E���g
            if (!isPlaying)
                timer += Time.deltaTime;

            // ��莞�Ԃ��Ƃɕ��˂��s��
            if (timer >= steamInterval)
            {
                // �v���C���[���G���A���ɂ���ꍇ�A��ɔ�΂�
                if (isPlayerInArea && playerRb != null)
                {
                    LaunchPlayer();
                }

                // ���C�G�t�F�N�g��\��
                StartCoroutine(ShowSmokeEffect());

                timer = 0f; // �^�C�}�[�����Z�b�g
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                playerRb = other.gameObject.GetComponent<Rigidbody>();
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

        private IEnumerator ShowSmokeEffect()
        {
            isPlaying = true;
            smokeEffect.SetActive(true); // ���C�G�t�F�N�g��\��
            yield return new WaitForSeconds(smokeDuration); // �w�莞�ԑҋ@
            smokeEffect.SetActive(false); // ���C�G�t�F�N�g���\��
            isPlaying = false;

        }
    }
}
