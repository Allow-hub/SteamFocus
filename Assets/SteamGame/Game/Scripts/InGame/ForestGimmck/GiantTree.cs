using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantTree : MonoBehaviour
{
    public float moveSpeed = 5f;  // �؂̏��i�ރX�s�[�h

    private bool isPlayerOnTree = false;  // �v���C���[���؂ɏ���Ă��邩

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTree = true;  // �v���C���[���؂ɏ����
            other.transform.SetParent(transform);  // �v���C���[��؂̐e�ɐݒ肵�Ĉꏏ�ɓ�����
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTree = false;  // �v���C���[���؂���~�肽
            other.transform.SetParent(null);  // �e�ݒ������
        }
    }

    void Update()
    {
        if (isPlayerOnTree)
        {
            // �v���C���[���؂ɏ���Ă���ꍇ�A�؂ƈꏏ�ɐi��
            float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * move);
        }
    }
}
