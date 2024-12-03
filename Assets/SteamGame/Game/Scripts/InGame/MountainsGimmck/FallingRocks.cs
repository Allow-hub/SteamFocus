using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [Header("Rock Settings")]
    public List<GameObject> rockObjects;  // �C���X�y�N�^�[�Őݒ肷���I�u�W�F�N�g�̃��X�g
    public float fallInterval = 2.0f;    // �₪������Ԋu
    public float spawnHeight = 10.0f;    // �₪��������鍂��
    public float fallDelay = 1.5f;       // �₪�������J�n����܂ł̒x������

    private bool isPlayerInZone = false; // �v���C���[���G���A���ɂ��邩
    private int nextRockIndex = 0;       // ���Ɏg�p�����̃C���f�b�N�X

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            StartCoroutine(DropRocks(other.transform));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    IEnumerator DropRocks(Transform playerTransform)
    {
        while (isPlayerInZone)
        {
            if (rockObjects.Count == 0)
            {
                Debug.LogError("Rock objects are not set in the inspector!");
                yield break;
            }

            // �g�p�������擾
            GameObject rock = rockObjects[nextRockIndex];
            nextRockIndex = (nextRockIndex + 1) % rockObjects.Count;

            // �v���C���[�̏�Ɋ��z�u
            Vector3 spawnPosition = playerTransform.position + Vector3.up * spawnHeight;
            rock.transform.position = spawnPosition;
            rock.SetActive(true);

            Debug.Log("Rock spawned at: " + spawnPosition);

            // �����R���[�`�����J�n
            StartCoroutine(FallRock(rock));

            // ���̊�𗎂Ƃ��܂őҋ@
            yield return new WaitForSeconds(fallInterval);
        }
    }

    IEnumerator FallRock(GameObject rock)
    {
        // �����܂ł̒x��
        yield return new WaitForSeconds(fallDelay);

        // ��𗎉�������
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // ��莞�Ԍ�Ɋ�����Z�b�g���Ĕ�\��
        yield return new WaitForSeconds(3.0f);
        rb.isKinematic = true;
        rock.SetActive(false); // ����\���ɂ��čė��p�\�ɂ���
    }
}

public class RockCollision : MonoBehaviour
{
    public float knockbackForce = 10000f; // �{�[���𐁂���΂���

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                // �Փ˕������v�Z�i�������̂݁j
                Vector3 knockbackDirection = Vector3.right; // �f�t�H���g��X�����ɔ�΂�
                if (Random.value > 0.5f)
                {
                    knockbackDirection = Vector3.left; // 50%�̊m���ō������ɔ�΂�
                }

                // �{�[���ɗ͂�������
                ballRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                Debug.Log("Ball hit by rock and knocked sideways!");
            }
        }
    }
}
