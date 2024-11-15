using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandTimer : MonoBehaviour
{
    public float timeLimit = 10.0f; // 制限時間
    private float timer;
    private bool isTiming = false;

    public Text timerText; // 制限時間表示用UI（オプション）

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (isTiming)
        {
            timer -= Time.deltaTime;

            // タイマーをUIに表示
            if (timerText != null)
            {
                timerText.text = "Time Left: " + Mathf.Max(0, timer).ToString("F2");
            }

            // 時間が0になったら失敗
            if (timer <= 0)
            {
                isTiming = false;
                FailAction();
            }
        }
    }

    public void StartTimer()
    {
        isTiming = true;
        timer = timeLimit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTimer(); // プレイヤーがエリアに入るとタイマーがスタート
        }
    }

    void FailAction()
    {
        // タイムアウト時の失敗アクション（例：リスポーンやメッセージ表示）
        Debug.Log("Time's up! You failed.");
    }
}
