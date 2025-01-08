using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [Header("Rock Settings")]
    [SerializeField]private List<GameObject> rockObjects;  // �C���X�y�N�^�[�Őݒ肷���I�u�W�F�N�g�̃��X�g
    [SerializeField] private float fallInterval = 2.0f;    // �₪������Ԋu
    [SerializeField] private float spawnHeight = 10.0f;    // �₪��������鍂��
    [SerializeField] private float fallDelay = 1.5f;       // �₪�������J�n����܂ł̒x������
    [SerializeField] private Vector3 spawnAreaCenter;      // �₪������G���A�̒��S
    [SerializeField] private Vector3 spawnAreaSize;        // �₪������G���A�̃T�C�Y

    private int nextRockIndex = 0;       // ���Ɏg�p�����̃C���f�b�N�X
    [SerializeField] private GameObject spawnAreaObject; // �X�|�[���G���A�p�I�u�W�F�N�g

    void Start()
    {
        if (spawnAreaObject != null)
        {
            // �X�|�[���G���A�̒��S�ƃT�C�Y���擾
            spawnAreaCenter = spawnAreaObject.transform.position;
            spawnAreaSize = spawnAreaObject.GetComponent<BoxCollider>().size;
        }

        StartCoroutine(DropRocks());
    }

    IEnumerator DropRocks()
    {
        while (true)
        {
            if (rockObjects.Count == 0)
            {
                Debug.LogError("Rock objects are not set in the inspector!");
                yield break;
            }

            // �g�p�������擾
            GameObject rock = rockObjects[nextRockIndex];
            nextRockIndex = (nextRockIndex + 1) % rockObjects.Count;

            // �����_���Ȉʒu�Ɋ�𐶐�
            Vector3 spawnPosition = GetRandomSpawnPosition();
            spawnPosition.y += spawnHeight; // Y�����𒲐�
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
        yield return new WaitForSeconds(4.5f);
        rb.isKinematic = true;
        rock.SetActive(false); // ����\���ɂ��čė��p�\�ɂ���
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // �X�|�[���G���A���Ń����_���Ȉʒu�𐶐�
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float z = Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);

        return new Vector3(x, spawnAreaCenter.y, z);
    }
}
