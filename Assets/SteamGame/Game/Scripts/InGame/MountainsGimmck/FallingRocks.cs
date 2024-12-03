using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [Header("Rock Settings")]
    public List<GameObject> rockObjects;  // インスペクターで設定する岩オブジェクトのリスト
    public float fallInterval = 2.0f;    // 岩が落ちる間隔
    public float spawnHeight = 10.0f;    // 岩が生成される高さ
    public float fallDelay = 1.5f;       // 岩が落下を開始するまでの遅延時間

    private bool isPlayerInZone = false; // プレイヤーがエリア内にいるか
    private int nextRockIndex = 0;       // 次に使用する岩のインデックス

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

            // 使用する岩を取得
            GameObject rock = rockObjects[nextRockIndex];
            nextRockIndex = (nextRockIndex + 1) % rockObjects.Count;

            // プレイヤーの上に岩を配置
            Vector3 spawnPosition = playerTransform.position + Vector3.up * spawnHeight;
            rock.transform.position = spawnPosition;
            rock.SetActive(true);

            Debug.Log("Rock spawned at: " + spawnPosition);

            // 落下コルーチンを開始
            StartCoroutine(FallRock(rock));

            // 次の岩を落とすまで待機
            yield return new WaitForSeconds(fallInterval);
        }
    }

    IEnumerator FallRock(GameObject rock)
    {
        // 落下までの遅延
        yield return new WaitForSeconds(fallDelay);

        // 岩を落下させる
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 一定時間後に岩をリセットして非表示
        yield return new WaitForSeconds(3.0f);
        rb.isKinematic = true;
        rock.SetActive(false); // 岩を非表示にして再利用可能にする
    }
}

public class RockCollision : MonoBehaviour
{
    public float knockbackForce = 10000f; // ボールを吹っ飛ばす力

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                // 衝突方向を計算（横方向のみ）
                Vector3 knockbackDirection = Vector3.right; // デフォルトでX方向に飛ばす
                if (Random.value > 0.5f)
                {
                    knockbackDirection = Vector3.left; // 50%の確率で左方向に飛ばす
                }

                // ボールに力を加える
                ballRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                Debug.Log("Ball hit by rock and knocked sideways!");
            }
        }
    }
}
