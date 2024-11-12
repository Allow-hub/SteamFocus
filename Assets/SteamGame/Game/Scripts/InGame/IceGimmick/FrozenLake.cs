using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLake : MonoBehaviour
{
    public float timeToCrack = 3.0f;  // �X�������܂ł̎���
    public float timeToSinkAfterCrack = 2.0f;  // ����Ă��璾�ނ܂ł̎���
    private bool isCracking = false;
    private bool isCracked = false;
    private bool isSinking = false;

    [Header("Cracking Effects")]
    public AudioClip crackingSound;    // ����鉹
    public GameObject crackEffect;     // �Ђъ���G�t�F�N�g
    public GameObject[] crackStages;   // �Ђъ���̒i�K��\���I�u�W�F�N�g�i�X�v���C�g��G�t�F�N�g�j
    public AudioClip sinkSound;        // ���މ�

    private enum CrackState
    {
        None,
        Cracking,
        Cracked,
        Sinking
    }

    private CrackState currentCrackState = CrackState.None;
    private bool[] crackStageStatus;   // �e�Ђъ���i�K�̏�Ԃ�ێ�����z��

    void Start()
    {
        crackStageStatus = new bool[crackStages.Length];

        // �ŏ��̂Ђъ���i�K�������A�N�e�B�u�ɂ���
        if (crackStages.Length > 0)
        {
            crackStages[0].SetActive(true);
            crackStageStatus[0] = true;
        }
        else
        {
            Debug.LogWarning("No crack stages assigned!");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCracking)
        {
            isCracking = true;
            StartCoroutine(CrackAndProgress());
        }
    }

    IEnumerator CrackAndProgress()
    {
        float timeElapsed = 0f;

        // �Ђъ�����J�n
        while (timeElapsed < timeToCrack)
        {
            currentCrackState = CrackState.Cracking;
            UpdateCrackStage(timeElapsed / timeToCrack);  // ����̐i�s�x�ɉ����Ēi�K�I�ɕω�
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // ���ꂽ���
        currentCrackState = CrackState.Cracked;
        UpdateCrackStage(1f);  // ���S�Ɋ��ꂽ��Ԃ�\��

        // ���ꂽ��ɒ��ނ܂ő҂�
        yield return new WaitForSeconds(timeToSinkAfterCrack);

        // ����
        currentCrackState = CrackState.Sinking;
        SinkIce();

        // �Ō�ɕX�������i�폜������A�N�e�B�u�ɂ���j
        gameObject.SetActive(false);
    }

    void UpdateCrackStage(float progress)
    {
        int stageToActivate = Mathf.FloorToInt(progress * crackStages.Length);

        for (int i = 0; i < crackStages.Length; i++)
        {
            if (i < stageToActivate && !crackStageStatus[i])
            {
                crackStages[i].SetActive(true);  // �Ђъ���i�K���A�N�e�B�u��
                crackStageStatus[i] = true;     // ���̒i�K�����ꂽ���Ƃ��L�^
            }
            else if (i >= stageToActivate && crackStageStatus[i])
            {
                crackStages[i].SetActive(false);  // ����Ă��Ȃ��i�K���A�N�e�B�u��
                crackStageStatus[i] = false;     // ���̒i�K�����Z�b�g
            }
        }

        // ����鉹���Đ�
        if (crackingSound != null && currentCrackState == CrackState.Cracking)
        {
            AudioSource.PlayClipAtPoint(crackingSound, transform.position);
        }
    }

    void SinkIce()
    {
        // ���މ����Đ�
        if (sinkSound != null)
        {
            AudioSource.PlayClipAtPoint(sinkSound, transform.position);
        }

        // ���ރG�t�F�N�g��A�j���[�V������ǉ�����ꍇ
        // ��: ParticleSystem��T�E���h���Đ�
    }

    void OnCollisionExit(Collision collision)
    {
        // �v���C���[���X���痣�ꂽ�Ƃ��Ɋ���̐i�s�𒆒f���Ȃ�
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[������Ă��i�s���̊����Ԃ�ێ�
            if (currentCrackState == CrackState.Cracked || currentCrackState == CrackState.Sinking)
            {
                StopAllCoroutines();
            }
        }
    }
}
