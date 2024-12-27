using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class LaserController : MonoBehaviour
    {
        [Header("���[�U�[�C�x���g")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private Transform targetPos; // ����������߂����ꏊ
        [SerializeField] private GameObject[] laser;
        [SerializeField] private Transform[] movePos;
        [SerializeField] private float[] moveSpeed; // �e���[�U�[�̈ړ����x

        [SerializeField] private GameObject wall;
        [SerializeField] private float wallSpeed;
        [SerializeField] private float wallY;

        private Vector3[] laserInitialPositions; // �e���[�U�[�̏����ʒu��ۑ�
        private Transform wallInitPos;
        private bool IsPlaying = false;
        private bool canPlayNextLaser = true;
        private const int eventCount = 3;
        private float proximityThreshold = 0.1f; // ���̃C�x���g���J�n���邽�߂̋����̂������l

        private void Awake()
        {
            if (laser.Length != eventCount || movePos.Length != eventCount || moveSpeed.Length != eventCount)
            {
                Debug.LogError("Laser��movePos�̐����z��ƈႢ�܂��A�ݒ���������Ă�������");
            }

            // �e���[�U�[�̏����ʒu��ۑ�
            laserInitialPositions = new Vector3[laser.Length];
            for (int i = 0; i < laser.Length; i++)
            {
                laserInitialPositions[i] = laser[i].transform.position;
            }

            wallInitPos = wall.transform;
            IsPlaying = false;
        }

        private IEnumerator LaserEvent()
        {
            if (IsPlaying) yield break;
            IsPlaying = true;
            wall.SetActive(true);
            Vector3 targetWallPosition = new Vector3(wall.transform.position.x, wall.transform.position.y + wallY, wall.transform.position.z);

            // wall���^�[�Q�b�g�ʒu�ɓ��B����܂ňړ�
            while (Vector3.Distance(wall.transform.position, targetWallPosition) > proximityThreshold)
            {
                wall.transform.position = Vector3.MoveTowards(wall.transform.position, targetWallPosition, wallSpeed * Time.deltaTime);
                yield return null;
            }

            // �����҂��Ă��烌�[�U�[�̃V�[�P���X���J�n
            yield return new WaitForSeconds(5f);

            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].SetActive(true);

                // movePos[i] �̈ʒu�Ƀ��[�U�[���ړ�������
                while (Vector3.Distance(laser[i].transform.position, movePos[i].position) > proximityThreshold)
                {
                    laser[i].transform.position = Vector3.MoveTowards(laser[i].transform.position, movePos[i].position, moveSpeed[i] * Time.deltaTime);
                    yield return null;
                }

                laser[i].SetActive(false);
                yield return new WaitForSeconds(1f); // ���̃��[�U�[�C�x���g��ҋ@
            }

            // �C�x���g�����������烌�[�U�[�������ʒu�ɖ߂�
            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].transform.position = laserInitialPositions[i];
            }

            // wall�������ʒu�ɖ߂�
            wall.transform.position = wallInitPos.position;
            wall.SetActive(false);

            IsPlaying = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                StartCoroutine(LaserEvent());
            }
        }

        // ���������Ƃ��̏���
        public void TouchLaser(GameObject obj)
        {
            Debug.Log("Touch");
            //obj.transform.position = targetPos.position;
        }
    }
}
