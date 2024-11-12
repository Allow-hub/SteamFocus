using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionPit : MonoBehaviour
{
    public float pullStrength = 5.0f;       // �v���C���[�𒆐S�Ɉ����񂹂�͂̋���
    public float sinkSpeed = 0.2f;          // �v���C���[�����ޑ��x
    public float fallDepth = -5.0f;         // �������n�܂�Y���W�̐[��
    private Transform pitCenter;            // �a�n���̒��S�ʒu
    private Rigidbody playerRigidbody;      // �v���C���[��Rigidbody�ւ̎Q��
    private bool isPlayerInPit = false;

    void Start()
    {
        // �a�n���̒��S�ʒu��ݒ�i�I�u�W�F�N�g�̒��S�j
        pitCenter = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[���a�n���ɓ������Ƃ�
        if (other.CompareTag("Player"))
        {
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                isPlayerInPit = true;
                playerRigidbody.drag = 5.0f;  // �����̂悤�ɋ�C��R�𑝉������ē�����݂�����
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[���a�n������o���Ƃ�
        if (other.CompareTag("Player") && playerRigidbody != null)
        {
            isPlayerInPit = false;
            playerRigidbody.drag = 0.0f;  // ��C��R�����ɖ߂�
            playerRigidbody = null;
        }
    }

    void Update()
    {
        // �v���C���[���a�n�����ɂ���ꍇ
        if (isPlayerInPit && playerRigidbody != null)
        {
            // 1. ���S�ւ̈�����
            Vector3 directionToCenter = (pitCenter.position - playerRigidbody.position).normalized;
            float distance = Vector3.Distance(playerRigidbody.position, pitCenter.position);
            float adjustedPullStrength = pullStrength / Mathf.Max(1.0f, distance); // ���S�ɋ߂Â��قǋ��������񂹂�
            playerRigidbody.AddForce(directionToCenter * adjustedPullStrength, ForceMode.Acceleration);

            // 2. ���̏�ł̒���
            playerRigidbody.MovePosition(playerRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);

            // 3. ���̐[���ɒB�����痎��������
            if (playerRigidbody.position.y <= fallDepth)
            {
                playerRigidbody.drag = 0.0f;  // ��C��R�����ɖ߂�
                playerRigidbody.AddForce(Vector3.down * 10.0f, ForceMode.VelocityChange);  // �����I�ɗ���������
                isPlayerInPit = false;  // ������ɒ��ޏ������~�߂�
            }
        }
    }
}
