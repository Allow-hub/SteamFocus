using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackingIceFloor : MonoBehaviour
{
    public float crackTime = 2f;    // �Ђъ��ꂪ�n�܂�܂ł̎���
    public float breakTime = 4f;    // ���S�Ɋ����܂ł̎���
    private bool isCracking = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCracking)
        {
            isCracking = true;
            StartCoroutine(CrackAndBreak());
        }
    }

    IEnumerator CrackAndBreak()
    {
        // �Ђъ���A�j���[�V�����̊J�n
        yield return new WaitForSeconds(crackTime);
        Debug.Log("Ice is cracking!");

        // �����܂ł̒x��
        yield return new WaitForSeconds(breakTime - crackTime);
        Debug.Log("Ice has broken!");
        Destroy(gameObject);
    }
}
