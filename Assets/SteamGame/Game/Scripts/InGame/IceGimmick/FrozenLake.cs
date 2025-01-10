using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLake : MonoBehaviour
{
    public float timeToCrack = 3.0f;           // 各段階でのひび割れまでの時間
    public AudioClip crackingSound;            // 割れる音
    public GameObject[] crackStages;           // ひび割れの段階を表すオブジェクト

    private int currentStageIndex = 0;         // 現在のひび割れ段階
    private bool isCracking = false;           // ひび割れ中かどうか
    private bool isPlayerNear = false;         // プレイヤーが氷に触れているかどうか

    void Start()
    {
        // 最初のひび割れ段階だけをアクティブにする
        SetCrackStage(0);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // プレイヤーが氷に触れているとき、ひび割れが進行中でなければ
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
        // プレイヤーが氷から離れた時
        if (collision.gameObject.CompareTag("Ball"))
        {
            isPlayerNear = false;  // プレイヤーが離れたことを記録
            StopAllCoroutines();   // ひび割れの進行を止める（コルーチンを中断）
        }
    }

    IEnumerator CrackProgress()
    {
        // 各段階でのひび割れを順に進行させる
        while (currentStageIndex < crackStages.Length)
        {
            // 割れる音を再生
            if (crackingSound != null)
            {
                AudioSource.PlayClipAtPoint(crackingSound, transform.position);
            }

            yield return new WaitForSeconds(timeToCrack);

            // 現在のひび割れ段階を非表示にし、次の段階を表示
            SetCrackStage(currentStageIndex + 1);
            currentStageIndex++;
        }

        // 全段階が進行したら氷を消す
        DestroyIce();
    }

    void SetCrackStage(int stageIndex)
    {
        // 全段階を非アクティブにして指定した段階のみアクティブにする
        for (int i = 0; i < crackStages.Length; i++)
        {
            crackStages[i].SetActive(i == stageIndex);
        }
    }

    void DestroyIce()
    {
        // 氷のオブジェクトを消去
        gameObject.SetActive(false);
    }

    void Update()
    {
        // プレイヤーが再び近づいた場合、ひび割れの進行を再開する
        if (isPlayerNear && !isCracking)
        {
            isCracking = true;
            StartCoroutine(CrackProgress());
        }
    }
}
