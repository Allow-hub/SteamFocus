using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingIcicleTrap : MonoBehaviour
{
    [Header("Icicle Settings")]
    public List<GameObject> icicleObjects;  // �C���X�y�N�^�[�Őݒ肷��X���I�u�W�F�N�g�̃��X�g
    public float fallInterval = 2.0f;       // �X����������Ԋu
    public float spawnHeight = 10.0f;       // �X������������鍂��
    public float fallDelay = 1.5f;          // �X�����������J�n����܂ł̒x������

    [Header("Trap Zone Settings")]
    public Vector3 zoneCenter = Vector3.zero;   // �G���A�̒��S�ʒu
    public Vector3 zoneSize = new Vector3(10f, 0, 10f);  // �G���A�̃T�C�Y (X, Z)

    private bool isPlayerInZone = false;
    private int nextIcicleIndex = 0;            // ���Ɏg�p����X���̃C���f�b�N�X

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            StartCoroutine(DropIcicles(other.transform));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    IEnumerator DropIcicles(Transform playerTransform)
    {
        while (isPlayerInZone)
        {
            if (icicleObjects.Count == 0)
            {
                Debug.LogError("Icicle objects are not set in the inspector!");
                yield break;
            }

            // �g�p����X�����擾
            GameObject icicle = icicleObjects[nextIcicleIndex];
            nextIcicleIndex = (nextIcicleIndex + 1) % icicleObjects.Count;

            // �v���C���[�̎��͂Ƀ����_���Ȉʒu��z�u
            Vector3 spawnPosition = GetRandomPositionNearPlayer(playerTransform) + Vector3.up * spawnHeight;
            icicle.transform.position = spawnPosition;
            icicle.SetActive(true); // �X����\��

            Debug.Log("Icicle spawned at: " + spawnPosition);  // �f�o�b�O���b�Z�[�W

            // �����R���[�`�����J�n
            StartCoroutine(FallIcicle(icicle));

            // ���̕X���𗎂Ƃ��܂őҋ@
            yield return new WaitForSeconds(fallInterval);
        }
    }

    Vector3 GetRandomPositionNearPlayer(Transform playerTransform)
    {
        // �v���C���[�̎��͂Ń����_���Ȉʒu������i�͈͂͒����\�j
        float rangeX = 5f;  // X�������͈̔�
        float rangeZ = 5f;  // Z�������͈̔�
        float randomX = Random.Range(playerTransform.position.x - rangeX, playerTransform.position.x + rangeX);
        float randomZ = Random.Range(playerTransform.position.z - rangeZ, playerTransform.position.z + rangeZ);

        return new Vector3(randomX, playerTransform.position.y, randomZ);
    }

    IEnumerator FallIcicle(GameObject icicle)
    {
        // �����܂ł̒x��
        yield return new WaitForSeconds(fallDelay);

        // �X���𗎉�������
        Rigidbody rb = icicle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // ��莞�Ԍ�ɕX�������Z�b�g���Ĕ�\��
        yield return new WaitForSeconds(3.0f);
        rb.isKinematic = true;
        icicle.SetActive(false); // �X�����\���ɂ��čė��p�\�ɂ���
    }

    // �G���A�͈̔͂����o�I�ɕ\���i�M�Y���`��p�j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(zoneCenter + transform.position, zoneSize);
    }
}
