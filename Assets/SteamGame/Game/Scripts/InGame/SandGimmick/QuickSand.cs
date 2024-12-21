using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand : MonoBehaviour
{
    public float sinkSpeed = 0.5f;  // �v���C���[�����ޑ��x
    public float increasedDrag = 20.0f;  // �������ł̋�C��R
    public float jumpBoost = 10.0f;  // �W�����v�͂𑝉��������
    private Rigidbody ballRigidbody;  // �v���C���[��Rigidbody�ւ̎Q��
    private bool isPlayerInQuicksand = false;

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[�������ɓ������Ƃ�
        if (other.CompareTag("Ball"))
        {
            ballRigidbody = other.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                isPlayerInQuicksand = true;
                ballRigidbody.drag = increasedDrag;  // ��C��R�𑝉�������
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[����������o���Ƃ�
        if (other.CompareTag("Ball") && ballRigidbody != null)
        {
            isPlayerInQuicksand = false;
            ballRigidbody.drag = 0.0f;  // ��C��R�����ɖ߂�
            ballRigidbody = null;
        }
    }

    void Update()
    {
        if (isPlayerInQuicksand && ballRigidbody != null)
        {
            // �v���C���[�����X��Y�������ɒ��߂�
            //ballRigidbody.MovePosition(ballRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);

            // �W�����v���ɃW�����v�͂𑝉�������
            if (Input.GetButtonDown("Jump"))
            {
                ballRigidbody.AddForce(Vector3.up * jumpBoost, ForceMode.Impulse);
            }
        }
    }
}
