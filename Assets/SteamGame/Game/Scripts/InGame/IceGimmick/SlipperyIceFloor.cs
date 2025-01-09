using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlipperyIceFloor : MonoBehaviour
{
    public float slipForce = 10f;  // �����
    public float dragOnIce = 1f;   // �X�̏�̃h���b�O�l

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // �v���C���[���X�̏��ɐG�ꂽ�ꍇ�ARigidbody�̃h���b�O��ύX���Ċ���₷������
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.drag = dragOnIce;  // �X�̏�Ŋ���悤�Ƀh���b�O��ݒ�
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �v���C���[���X�̏��ɂ���ԁA����͂�������
                Debug.Log("OnCollisionStay: Applying slip force");
                Vector3 slipDirection = playerRb.velocity.normalized;
                playerRb.AddForce(slipDirection * slipForce, ForceMode.Force);
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[���X�̏����痣�ꂽ��A�h���b�O�����ɖ߂�
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.drag = 0;  // ���ʂ̏��Ɠ����悤�ɖ߂�
            }
        }
    }
}
