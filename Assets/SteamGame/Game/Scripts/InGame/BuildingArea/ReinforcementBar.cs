using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ReinforcementBar : MonoBehaviour
    {
        [Header("�ォ��S�ؗ��Ƃ�")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool;

        [Header("Settings")]
        [SerializeField] private Vector2 intervalRange;
        [SerializeField] private Vector2 forceRange;
        [SerializeField] private Vector2 createRange;
        [SerializeField] private GameObject[] createObj;
        [SerializeField] private float appearDuration;
        [SerializeField] private Vector3 diraction;

        [Header("�l�p�`�G���A�ݒ�")]
        [SerializeField] private Transform centerPosition; // �l�p�`�̒��S
        [SerializeField] private Vector2 areaSize; // �l�p�`�̕��ƍ���


        private int currentCreateNum;
        private Vector3 createPos;
        private float currentForce;
        private float currentInterval;
        private float elapsedTime = 0;

        private void OnValidate()
        {
            objectPool = FindObjectOfType<ObjectPool>();
        }
        private void Awake()
        {
            Lottery();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > currentInterval)
            {
                for (int i = 0; i < currentCreateNum; i++)
                    CreateObj();
                Lottery();
            }
        }

        private void Lottery()
        {
            // �l�p�`�G���A���̃����_���ȓ_�𒊑I
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);

            createPos = centerPosition.position + new Vector3(randomX, 0, randomY);

            // �����_���ȗ͂ƃC���^�[�o����ݒ�
            currentForce = Random.Range(forceRange.x, forceRange.y);
            currentInterval = Random.Range(intervalRange.x, intervalRange.y);
            currentCreateNum = (int)Random.Range(createRange.x, createRange.y);

            elapsedTime = 0;
        }
        private void CreateObj()
        {
            // ObjectPool ���g�����I�u�W�F�N�g��������
            int n = Random.Range(0, createObj.Length);

            GameObject obj = objectPool.GetObject(createObj[n]);
            StartCoroutine(ReturnObj(obj));
            if (obj != null)
            {
                // �����_���Ȉʒu���v�Z
                float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
                float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
                Vector3 randomPosition = centerPosition.position + new Vector3(randomX, 0, randomY);
                obj.transform.position = randomPosition;

                // �����_���ȉ�]��ݒ�
                //Quaternion randomRotation = Random.rotation; // �����_���ȉ�]�𐶐�
                //obj.transform.rotation = randomRotation;

                // ���˕����Ƀ����_���ȃI�t�Z�b�g��������
                Vector3 randomizedDirection = diraction.normalized + new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    0,
                    Random.Range(-0.1f, 0.1f)).normalized;

                // �����_���ȗ͂�������
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(randomizedDirection * currentForce, ForceMode.Impulse);
                }
            }
        }



        private IEnumerator ReturnObj(GameObject obj)
        {
            yield return new WaitForSeconds(appearDuration);
            objectPool.ReturnObject(obj);
        }

        private void OnDrawGizmos()
        {
            // �l�p�`�G���A��`��
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(centerPosition.position, new Vector3(areaSize.x, 0, areaSize.y));

            // diraction�̕����ɐ���`��
            Gizmos.color = Color.red;
            Vector3 startPoint = centerPosition.position;
            Vector3 endPoint = startPoint + diraction.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}