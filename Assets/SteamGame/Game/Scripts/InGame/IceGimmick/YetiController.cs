using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    public GameObject snowballPrefab; // ��ʂ�Prefab
    public Transform snowballSpawnPoint; // ��ʂ̐����ʒu
    public float snowballForce = 10f; // ��ʂ�]���������x
    public float autoRollInterval = 5f; // ��ʂ�]�����Ԋu
    public float snowballDestroyTime = 10f; // ��ʂ�������܂ł̎���

    private void Start()
    {
        // ��ʂ������œ]�����R���[�`�����J�n
        StartCoroutine(AutoRollSnowball());
    }

    private IEnumerator AutoRollSnowball()
    {
        while (true)
        {
            RollSnowball(); // ��ʂ�]����
            yield return new WaitForSeconds(autoRollInterval); // �w�莞�ԑҋ@
        }
    }

    private void RollSnowball()
    {
        // ��ʂ𐶐�
        GameObject snowball = Instantiate(snowballPrefab, snowballSpawnPoint.position, Quaternion.identity);

        // ��ʂ̎Ζʕ����ɏ����x��^����
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // �Ζʂ̌������擾
            Vector3 slopeDirection = GetSlopeDirection(snowballSpawnPoint.position);
            rb.AddForce(slopeDirection * snowballForce, ForceMode.Impulse); // ��ʂɏ����x��^����
        }

        // ��莞�Ԍ�ɐ�ʂ��폜
        Destroy(snowball, snowballDestroyTime);
    }

    private Vector3 GetSlopeDirection(Vector3 position)
    {
        // �Ζʂ̖@�����擾
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 5f)) // �n�ʂɌ�����Raycast�ŎΖʂ̖@�����擾
        {
            // �@������Ζʕ������v�Z
            Vector3 slopeDirection = Vector3.Cross(hit.normal, Vector3.Cross(Vector3.down, hit.normal));
            return slopeDirection.normalized; // �Ζʂɉ������i�s������Ԃ�
        }
        return Vector3.forward; // ������擾�ł��Ȃ������ꍇ�̃f�t�H���g����
    }
}
