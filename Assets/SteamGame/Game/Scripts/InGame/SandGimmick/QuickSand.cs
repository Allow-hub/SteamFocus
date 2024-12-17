using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand : MonoBehaviour
{
    public float sinkSpeed = 0.5f;  // �v���C���[�����ޑ��x
    public float increasedDrag = 20.0f;  // �������ł̋�C��R
    private Rigidbody playerRigidbody;  // �v���C���[��Rigidbody�ւ̎Q��
    private bool isPlayerInQuicksand = false;

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[�������ɓ������Ƃ�
        if (other.CompareTag("Ball"))
        {
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                isPlayerInQuicksand = true;
                playerRigidbody.drag = increasedDrag;  // ��C��R�𑝉�������
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[����������o���Ƃ�
        if (other.CompareTag("Ball") && playerRigidbody != null)
        {
            isPlayerInQuicksand = false;
            playerRigidbody.drag = 0.0f;  // ��C��R�����ɖ߂�
            playerRigidbody = null;
        }
    }

    void Update()
    {
        // �v���C���[���������ɂ���ꍇ�A���X��Y�������ɒ��߂�
        if (isPlayerInQuicksand && playerRigidbody != null)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);
        }
    }
}
