using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ShotCar : MonoBehaviour
    {
        [Header("�Ԃƃo�C�N���΂�")]
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
        [SerializeField] private Vector3 direction;

        [Header("�l�p�`�G���A�ݒ�")]
        [SerializeField] private Transform centerPosition; // �l�p�`�̒��S
        [SerializeField] private Vector2 areaSize; // �l�p�`�̕��ƍ���
        [SerializeField] private float rotationAngle; // �l�p�`�̉�]�p�x

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
                {
                    CreateObj();
                    Lottery();
                }
                EndLottery();
            }
        }

        private void Lottery()
        {
            // �l�p�`�G���A���̃����_���ȓ_�𒊑I
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);

            // �����_���ȓ_����]������
            Vector2 randomPoint = new Vector2(randomX, randomY);
            randomPoint = RotatePoint(randomPoint, rotationAngle);

            createPos = centerPosition.position + new Vector3(randomPoint.x, randomPoint.y, 0);

            // �����_���ȗ͂ƃC���^�[�o����ݒ�
            currentForce = Random.Range(forceRange.x, forceRange.y);
        }

        private void EndLottery()
        {

            currentInterval = Random.Range(intervalRange.x, intervalRange.y);
            currentCreateNum = (int)Random.Range(createRange.x, createRange.y);

            elapsedTime = 0;
        }

        private Vector2 RotatePoint(Vector2 point, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);

            float xNew = point.x * cos - point.y * sin;
            float yNew = point.x * sin + point.y * cos;

            return new Vector2(xNew, yNew);
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
                obj.transform.position = createPos;

                // �����_���ȉ�]��ݒ�
                Quaternion randomRotation = Random.rotation; // �����_���ȉ�]�𐶐�
                obj.transform.rotation = randomRotation;

                // ���˕����Ƀ����_���ȃI�t�Z�b�g��������
                Vector3 randomizedDirection = direction.normalized + new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f),
                    0).normalized;

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
            Vector3 size = new Vector3(areaSize.x, areaSize.y, 0);
            Vector3[] corners = new Vector3[4];
            corners[0] = centerPosition.position + new Vector3(-areaSize.x / 2, -areaSize.y / 2, 0);
            corners[1] = centerPosition.position + new Vector3(areaSize.x / 2, -areaSize.y / 2, 0);
            corners[2] = centerPosition.position + new Vector3(areaSize.x / 2, areaSize.y / 2, 0);
            corners[3] = centerPosition.position + new Vector3(-areaSize.x / 2, areaSize.y / 2, 0);

            Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
            for (int i = 0; i < 4; i++)
            {
                corners[i] = rotation * (corners[i] - centerPosition.position) + centerPosition.position;
            }

            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);

            // diraction�̕����ɐ���`��
            Gizmos.color = Color.red;
            Vector3 startPoint = centerPosition.position;
            Vector3 endPoint = startPoint + direction.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
