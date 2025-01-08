using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingFloor : MonoBehaviour
{
    public float tiltSpeed = 5.0f;         // �X�����x
    public float maxTiltAngle = 30.0f;    // �ő�X���p�x
    public float resetSpeed = 2.0f;       // ���ɖ߂鑬�x

    private Quaternion initialRotation;   // �����̏��̉�]
    private Transform player;             // �v���C���[��Transform
    private bool isPlayerOnFloor = false; // �v���C���[�����ɏ���Ă��邩�ǂ���

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�����ɏ�����ꍇ
        if (collision.collider.CompareTag("Ball"))
        {
            player = collision.transform;
            isPlayerOnFloor = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �v���C���[�������痣�ꂽ�ꍇ
        if (collision.collider.CompareTag("Ball"))
        {
            isPlayerOnFloor = false;
            player = null;
        }
    }

    void Update()
    {
        if (isPlayerOnFloor && player != null)
        {
            // �v���C���[�̈ʒu�ɉ����ď����X����
            Vector3 localPlayerPosition = transform.InverseTransformPoint(player.position);
            float tiltX = Mathf.Clamp(localPlayerPosition.z, -1, 1) * maxTiltAngle;
            float tiltZ = -Mathf.Clamp(localPlayerPosition.x, -1, 1) * maxTiltAngle;

            Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ) * initialRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
        else
        {
            // �v���C���[�����Ȃ��ꍇ�͌��ɖ߂�
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, resetSpeed * Time.deltaTime);
        }
    }
}
