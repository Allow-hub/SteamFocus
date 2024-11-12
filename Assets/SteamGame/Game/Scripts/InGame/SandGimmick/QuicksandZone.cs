using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuicksandZone : MonoBehaviour
{
    public float sinkSpeed = 0.1f;  // �v���C���[�����ޑ��x
    private bool isPlayerInQuicksand = false;
    private Transform playerTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInQuicksand = true;
            playerTransform = other.transform;  // �v���C���[��Transform���擾
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInQuicksand = false;
            playerTransform = null;  // �v���C���[���G���A����o��ƒ��ނ̂��~�߂�
        }
    }

    void Update()
    {
        if (isPlayerInQuicksand && playerTransform != null)
        {
            // �v���C���[���������ɏ��X�Ɉړ�������
            playerTransform.position += Vector3.down * sinkSpeed * Time.deltaTime;
        }
    }
}
