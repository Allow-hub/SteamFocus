using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSwamp : MonoBehaviour
{
    public Transform respawnPoint;   // �{�[���̃��X�|�[���n�_
    public float playerOffsetY = 1.0f; // �v���C���[���{�[���̏�ɔz�u���鍂���̃I�t�Z�b�g

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // �{�[���ƃv���C���[�����X�|�[��
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // �v���C���[������
            if (player != null)
            {
                StartCoroutine(RespawnBallAndPlayer(other.transform, player.transform));
            }
        }
    }

    IEnumerator RespawnBallAndPlayer(Transform ball, Transform player)
    {
        // 1�b�̒x����ǉ�
        yield return new WaitForSeconds(1f);

        // �{�[�������X�|�[���n�_�Ɉړ�
        ball.position = respawnPoint.position;
        ball.rotation = respawnPoint.rotation;

        // �v���C���[���{�[���̏㕔�ɔz�u
        Vector3 playerRespawnPosition = ball.position + Vector3.up * playerOffsetY;
        player.position = playerRespawnPosition; // �v���C���[�̈ʒu���{�[���Ɠ���
        player.rotation = ball.rotation;        // �v���C���[�̉�]���{�[���ɓ���

        Debug.Log("Ball and Player have been respawned at the correct location.");
    }
}
