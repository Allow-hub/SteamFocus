using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 2.0f;     // ����n�߂�܂ł̎���
    public float destroyDelay = 4.0f;    // ���S�ɏ��ł���܂ł̎���
    private bool isCrumbling = false;
    private Coroutine crumbleCoroutine;
    private Vector3 originalPosition;    // ���̈ʒu���L�^
    private Quaternion originalRotation; // ���̉�]���L�^

    void Start()
    {
        // �����ʒu�Ɖ�]��ۑ�
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[������ɏ�����Ƃ�
        if (collision.gameObject.CompareTag("Ball") && !isCrumbling)
        {
            isCrumbling = true;
            crumbleCoroutine = StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        // ����n�߂�܂ł̃f�B���C
        yield return new WaitForSeconds(crumbleDelay);

        // ���ꂪ����n�߂�A�j���[�V�����i��F���h��j
        float elapsedTime = 0f;
        while (elapsedTime < destroyDelay - crumbleDelay)
        {
            // �^�񒆂���ɉ��h���\��
            float swayAmount = Mathf.Sin(Time.time * 50f) * 0.5f; // �U���Ƒ��x�𒲐�
            transform.position = originalPosition + new Vector3(swayAmount, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���S�ɏ���
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        // �v���C���[�����ꂩ�痣�ꂽ�Ƃ��ɃR���[�`�����~
        if (collision.gameObject.CompareTag("Ball") && isCrumbling)
        {
            isCrumbling = false;
            if (crumbleCoroutine != null)
            {
                StopCoroutine(crumbleCoroutine);
                crumbleCoroutine = null;
            }

            // ��������̈ʒu�ɖ߂�
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}
