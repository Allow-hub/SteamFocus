using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLake : MonoBehaviour
{
    public float timeToCrack = 3.0f;           // �e�i�K�ł̂Ђъ���܂ł̎���
    public AudioClip crackingSound;            // ����鉹
    public GameObject[] crackStages;           // �Ђъ���̒i�K��\���I�u�W�F�N�g

    private int currentStageIndex = 0;         // ���݂̂Ђъ���i�K
    private bool isCracking = false;           // �Ђъ��ꒆ���ǂ���
    private bool isPlayerNear = false;         // �v���C���[���X�ɐG��Ă��邩�ǂ���

    void Start()
    {
        // �ŏ��̂Ђъ���i�K�������A�N�e�B�u�ɂ���
        SetCrackStage(0);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // �v���C���[���X�ɐG��Ă���Ƃ��A�Ђъ��ꂪ�i�s���łȂ����
            if (!isCracking && !isPlayerNear)
            {
                isPlayerNear = true;
                isCracking = true;
                StartCoroutine(CrackProgress());
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �v���C���[���X���痣�ꂽ��
        if (collision.gameObject.CompareTag("Ball"))
        {
            isPlayerNear = false;  // �v���C���[�����ꂽ���Ƃ��L�^
            StopAllCoroutines();   // �Ђъ���̐i�s���~�߂�i�R���[�`���𒆒f�j
        }
    }

    IEnumerator CrackProgress()
    {
        // �e�i�K�ł̂Ђъ�������ɐi�s������
        while (currentStageIndex < crackStages.Length)
        {
            // ����鉹���Đ�
            if (crackingSound != null)
            {
                AudioSource.PlayClipAtPoint(crackingSound, transform.position);
            }

            yield return new WaitForSeconds(timeToCrack);

            // ���݂̂Ђъ���i�K���\���ɂ��A���̒i�K��\��
            SetCrackStage(currentStageIndex + 1);
            currentStageIndex++;
        }

        // �S�i�K���i�s������X������
        DestroyIce();
    }

    void SetCrackStage(int stageIndex)
    {
        // �S�i�K���A�N�e�B�u�ɂ��Ďw�肵���i�K�̂݃A�N�e�B�u�ɂ���
        for (int i = 0; i < crackStages.Length; i++)
        {
            crackStages[i].SetActive(i == stageIndex);
        }
    }

    void DestroyIce()
    {
        // �X�̃I�u�W�F�N�g������
        gameObject.SetActive(false);
    }

    void Update()
    {
        // �v���C���[���Ăы߂Â����ꍇ�A�Ђъ���̐i�s���ĊJ����
        if (isPlayerNear && !isCracking)
        {
            isCracking = true;
            StartCoroutine(CrackProgress());
        }
    }
}
