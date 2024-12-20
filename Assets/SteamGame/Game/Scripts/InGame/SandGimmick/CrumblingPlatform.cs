using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 2.0f;     // 崩れ始めるまでの時間
    public float destroyDelay = 4.0f;    // 完全に消滅するまでの時間
    private bool isCrumbling = false;
    private Coroutine crumbleCoroutine;
    private Vector3 originalPosition;    // 元の位置を記録
    private Quaternion originalRotation; // 元の回転を記録

    void Start()
    {
        // 初期位置と回転を保存
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが足場に乗ったとき
        if (collision.gameObject.CompareTag("Ball") && !isCrumbling)
        {
            isCrumbling = true;
            crumbleCoroutine = StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        // 崩れ始めるまでのディレイ
        yield return new WaitForSeconds(crumbleDelay);

        // 足場が崩れ始めるアニメーション（例：横揺れ）
        float elapsedTime = 0f;
        while (elapsedTime < destroyDelay - crumbleDelay)
        {
            // 真ん中を基準に横揺れを表現
            float swayAmount = Mathf.Sin(Time.time * 50f) * 0.5f; // 振幅と速度を調整
            transform.position = originalPosition + new Vector3(swayAmount, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全に消滅
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        // プレイヤーが足場から離れたときにコルーチンを停止
        if (collision.gameObject.CompareTag("Ball") && isCrumbling)
        {
            isCrumbling = false;
            if (crumbleCoroutine != null)
            {
                StopCoroutine(crumbleCoroutine);
                crumbleCoroutine = null;
            }

            // 足場を元の位置に戻す
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}
