using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TechC
{
    public class ShotBomb : MonoBehaviour
    {
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private GameObject createPrefab;

        [SerializeField] private float appearTime = 10;
        [SerializeField] private int createNum = 1;       // 1�x�ɐ��������
        [SerializeField] private Transform createPosParent;
        [SerializeField] private Transform[] createPos;  // �����ʒu�̔z��
        [SerializeField] private float force;            // �I�u�W�F�N�g�ɗ^�����
        [SerializeField] private float interval;         // �����Ԋu
        [SerializeField] private float createInterval;   // �X�̐����Ԋu
        [SerializeField] private Vector3 direction;      // �I�u�W�F�N�g�̈ړ�����
        [SerializeField] private bool isDrawing = true;

        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed = 360f; // ��]���x�i�x/�b�j
        [SerializeField] private Vector3 rotateDirection;
        private int currentIndex = 0; // �z����̌��݂̃C���f�b�N�X

        private List<GameObject> activeObj = new List<GameObject>();

        private void OnValidate()
        {
            objectPool = FindObjectOfType<ObjectPool>();
            int cnildCount = createPosParent.transform.childCount;
            createPos = new Transform[cnildCount];
            for (int i = 0; i < cnildCount; i++)
            {
                createPos[i] = createPosParent.transform.GetChild(i).transform;
            }
        }

        private void Start()
        {
            StartCoroutine(CreateObjectsRoutine());
            StartCoroutine(ReturnOldObjectsRoutine()); // �Â��I�u�W�F�N�g��ԋp���郋�[�`�����J�n
        }

        private IEnumerator CreateObjectsRoutine()
        {
            while (true)
            {
                // �S�I�u�W�F�N�g�����ɐ���
                for (int i = 0; i < createNum; i++)
                {
                    CreateObj();
                    yield return new WaitForSeconds(createInterval); // �X�̐����Ԋu��ҋ@
                }

                // ���ׂĐ�����ɃC���^�[�o����ҋ@
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator ReturnOldObjectsRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(appearTime); // 5�b���Ƃɏ���
                if (activeObj.Count > 0)
                {
                    GameObject oldestObj = activeObj[0];
                    activeObj.RemoveAt(0); // ���X�g����폜
                    if (objectPool != null)
                    {
                        objectPool.ReturnObject(oldestObj); // �I�u�W�F�N�g�v�[���ɕԋp
                    }
                    else
                    {
                        Destroy(oldestObj); // �v�[�����Ȃ��ꍇ�͍폜
                    }
                }
            }
        }

        private void CreateObj()
        {
            // ���݂̃C���f�b�N�X�Ɋ�Â��Đ����ʒu������
            Transform spawnPoint = createPos[currentIndex];

            GameObject newObj = objectPool != null
                ? objectPool.GetObject(createPrefab)
                : Instantiate(createPrefab, spawnPoint.position, spawnPoint.rotation);
            var bomb = newObj.GetComponent<Bomb>();
            bomb.SetProperty(moveDirection, rotateDirection, moveSpeed, rotationSpeed);

            newObj.transform.position = spawnPoint.position;
            // �͂�^���� (Rigidbody������ꍇ)
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }

            // �A�N�e�B�u���X�g�ɒǉ�
            activeObj.Add(newObj);

            // ���̐����|�C���g�ɐi��
            currentIndex++;
            if (currentIndex >= createPos.Length)
            {
                currentIndex = 0; // �z��̍ŏ��ɖ߂�
            }
        }

        private void OnDrawGizmos()
        {
            if (!isDrawing) return;
            // diraction�̕����ɐ���`��
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + direction.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
