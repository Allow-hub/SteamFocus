using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 2.0f;     // 崩れ始めるまでの時間
    public float destroyDelay = 4.0f;     // 完全に消滅するまでの時間
    private bool isCrumbling = false;
    private Coroutine crumbleCoroutine;

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが足場に乗ったとき
        if (collision.gameObject.CompareTag("Player") && !isCrumbling)
        {
            isCrumbling = true;
            crumbleCoroutine = StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        // 崩れ始めるまでのディレイ
        yield return new WaitForSeconds(crumbleDelay);

        // 足場が崩れ始めるアニメーション（例：揺れ）
        float elapsedTime = 0f;
        while (elapsedTime < destroyDelay - crumbleDelay)
        {
            // 回転で揺れを表現
            transform.Rotate(new Vector3(0, 0, Mathf.Sin(Time.time * 20) * 1f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全に消滅
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        // プレイヤーが足場から離れたときにコルーチンを停止
        if (collision.gameObject.CompareTag("Player") && isCrumbling)
        {
            isCrumbling = false;
            if (crumbleCoroutine != null)
            {
                StopCoroutine(crumbleCoroutine);
                crumbleCoroutine = null;
            }
            // 足場が元の位置に戻るなどの処理を追加できます
        }
    }
}
