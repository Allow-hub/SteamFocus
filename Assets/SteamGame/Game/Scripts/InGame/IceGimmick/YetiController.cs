using System.Collections;
using System.Collections.Generic;
using TechC;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    public GameObject snowballPrefab; // ��ʂ�Prefab
    public Transform snowballSpawnPoint; // ��ʂ̐����ʒu
    public float snowballForce = 20f; // ��ʂ̏����x
    public float autoThrowInterval = 3f; // ��ʂ������œ�����Ԋu
    public Vector3 throwDirection = new Vector3(0, 1, 1); // ��ʂ𓊂�������i�΂ߏ�j

    public float snowballDestroyTime = 10f; // ��ʂ�������܂ł̎���

    private void Start()
    {
        // ��ʂ������œ�����R���[�`�����J�n
        StartCoroutine(AutoThrowSnowball());
    }

    private IEnumerator AutoThrowSnowball()
    {
        while (true)
        {
            ThrowSnowball(); // ��ʂ𓊂���
            yield return new WaitForSeconds(autoThrowInterval); // �w��Ԋu�őҋ@
        }
    }

    private void ThrowSnowball()
    {
        // ��ʂ𐶐�
        GameObject snowball = objectPool.GetObject(snowballPrefab);
        snowball.transform.position = snowballSpawnPoint.position;
        //snowball.transform.rotation =Quaternion.identity;

        // ��ʂ̌����Ə����x��ݒ�
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ����������𐳋K�����ė͂�������
            Vector3 normalizedDirection = throwDirection.normalized; // �x�N�g���𐳋K��
            rb.AddForce(normalizedDirection * snowballForce, ForceMode.Impulse); // ��ʂ𓊂���
        }

        // ��莞�Ԍ�ɐ�ʂ��폜
        //Destroy(snowball, snowballDestroyTime);
    }
}