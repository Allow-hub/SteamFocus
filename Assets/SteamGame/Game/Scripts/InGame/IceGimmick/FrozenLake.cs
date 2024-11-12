using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLake : MonoBehaviour
{
    public float timeToCrack = 3.0f;  // 氷が割れるまでの時間
    public float timeToSinkAfterCrack = 2.0f;  // 割れてから沈むまでの時間
    private bool isCracking = false;
    private bool isCracked = false;
    private bool isSinking = false;

    [Header("Cracking Effects")]
    public AudioClip crackingSound;    // 割れる音
    public GameObject crackEffect;     // ひび割れエフェクト
    public GameObject[] crackStages;   // ひび割れの段階を表すオブジェクト（スプライトやエフェクト）
    public AudioClip sinkSound;        // 沈む音

    private enum CrackState
    {
        None,
        Cracking,
        Cracked,
        Sinking
    }

    private CrackState currentCrackState = CrackState.None;
    private bool[] crackStageStatus;   // 各ひび割れ段階の状態を保持する配列

    void Start()
    {
        crackStageStatus = new bool[crackStages.Length];

        // 最初のひび割れ段階だけをアクティブにする
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

        // ひび割れを開始
        while (timeElapsed < timeToCrack)
        {
            currentCrackState = CrackState.Cracking;
            UpdateCrackStage(timeElapsed / timeToCrack);  // 割れの進行度に応じて段階的に変化
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 割れた状態
        currentCrackState = CrackState.Cracked;
        UpdateCrackStage(1f);  // 完全に割れた状態を表示

        // 割れた後に沈むまで待つ
        yield return new WaitForSeconds(timeToSinkAfterCrack);

        // 沈む
        currentCrackState = CrackState.Sinking;
        SinkIce();

        // 最後に氷を消す（削除せず非アクティブにする）
        gameObject.SetActive(false);
    }

    void UpdateCrackStage(float progress)
    {
        int stageToActivate = Mathf.FloorToInt(progress * crackStages.Length);

        for (int i = 0; i < crackStages.Length; i++)
        {
            if (i < stageToActivate && !crackStageStatus[i])
            {
                crackStages[i].SetActive(true);  // ひび割れ段階をアクティブに
                crackStageStatus[i] = true;     // この段階が割れたことを記録
            }
            else if (i >= stageToActivate && crackStageStatus[i])
            {
                crackStages[i].SetActive(false);  // 割れていない段階を非アクティブに
                crackStageStatus[i] = false;     // この段階をリセット
            }
        }

        // 割れる音を再生
        if (crackingSound != null && currentCrackState == CrackState.Cracking)
        {
            AudioSource.PlayClipAtPoint(crackingSound, transform.position);
        }
    }

    void SinkIce()
    {
        // 沈む音を再生
        if (sinkSound != null)
        {
            AudioSource.PlayClipAtPoint(sinkSound, transform.position);
        }

        // 沈むエフェクトやアニメーションを追加する場合
        // 例: ParticleSystemやサウンドを再生
    }

    void OnCollisionExit(Collision collision)
    {
        // プレイヤーが氷から離れたときに割れの進行を中断しない
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが離れても進行中の割れ状態を保持
            if (currentCrackState == CrackState.Cracked || currentCrackState == CrackState.Sinking)
            {
                StopAllCoroutines();
            }
        }
    }
}
