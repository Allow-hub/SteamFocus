using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 2.0f;     // ����n�߂�܂ł̎���
    public float destroyDelay = 4.0f;     // ���S�ɏ��ł���܂ł̎���
    private bool isCrumbling = false;
    private Coroutine crumbleCoroutine;

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[������ɏ�����Ƃ�
        if (collision.gameObject.CompareTag("Player") && !isCrumbling)
        {
            isCrumbling = true;
            crumbleCoroutine = StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        // ����n�߂�܂ł̃f�B���C
        yield return new WaitForSeconds(crumbleDelay);

        // ���ꂪ����n�߂�A�j���[�V�����i��F�h��j
        float elapsedTime = 0f;
        while (elapsedTime < destroyDelay - crumbleDelay)
        {
            // ��]�ŗh���\��
            transform.Rotate(new Vector3(0, 0, Mathf.Sin(Time.time * 20) * 1f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���S�ɏ���
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        // �v���C���[�����ꂩ�痣�ꂽ�Ƃ��ɃR���[�`�����~
        if (collision.gameObject.CompareTag("Player") && isCrumbling)
        {
            isCrumbling = false;
            if (crumbleCoroutine != null)
            {
                StopCoroutine(crumbleCoroutine);
                crumbleCoroutine = null;
            }
            // ���ꂪ���̈ʒu�ɖ߂�Ȃǂ̏�����ǉ��ł��܂�
        }
    }
}
