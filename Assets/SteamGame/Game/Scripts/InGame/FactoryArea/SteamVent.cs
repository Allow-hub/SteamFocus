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

        [SerializeField] private bool isShooting =false; //��ɏ��C���o�����ǂ���

        private bool isPlaying = false;

        private bool isPlayerInArea = false;
        private Rigidbody playerRb;
        private float timer;

        private void OnValidate()
        {
            if(!isShooting)
                smokeEffect.SetActive(false);
            else
                smokeEffect.SetActive(true);
        }

        private void Update()
        {
            if(isShooting)
            {
                // �v���C���[���G���A���ɂ���ꍇ�A��ɔ�΂�
                if (isPlayerInArea && playerRb != null)
                {
                    LaunchPlayer();
                }
            }
            else
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
            if (SeManager.I != null)
                SeManager.I.PlaySe(6, 0);
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
