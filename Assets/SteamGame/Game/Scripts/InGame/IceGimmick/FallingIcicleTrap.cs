using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingIcicleTrap : MonoBehaviour
{
    [Header("Icicle Settings")]
    public List<GameObject> icicleObjects;  // インスペクターで設定する氷柱オブジェクトのリスト
    public float fallInterval = 2.0f;       // 氷柱が落ちる間隔
    public float spawnHeight = 10.0f;       // 氷柱が生成される高さ
    public float fallDelay = 1.5f;          // 氷柱が落下を開始するまでの遅延時間

    [Header("Trap Zone Settings")]
    public Vector3 zoneCenter = Vector3.zero;   // エリアの中心位置
    public Vector3 zoneSize = new Vector3(10f, 0, 10f);  // エリアのサイズ (X, Z)

    private bool isPlayerInZone = false;
    private int nextIcicleIndex = 0;            // 次に使用する氷柱のインデックス

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

            // 使用する氷柱を取得
            GameObject icicle = icicleObjects[nextIcicleIndex];
            nextIcicleIndex = (nextIcicleIndex + 1) % icicleObjects.Count;

            // プレイヤーの周囲にランダムな位置を配置
            Vector3 spawnPosition = GetRandomPositionNearPlayer(playerTransform) + Vector3.up * spawnHeight;
            icicle.transform.position = spawnPosition;
            icicle.SetActive(true); // 氷柱を表示

            Debug.Log("Icicle spawned at: " + spawnPosition);  // デバッグメッセージ

            // 落下コルーチンを開始
            StartCoroutine(FallIcicle(icicle));

            // 次の氷柱を落とすまで待機
            yield return new WaitForSeconds(fallInterval);
        }
    }

    Vector3 GetRandomPositionNearPlayer(Transform playerTransform)
    {
        // プレイヤーの周囲でランダムな位置を決定（範囲は調整可能）
        float rangeX = 5f;  // X軸方向の範囲
        float rangeZ = 5f;  // Z軸方向の範囲
        float randomX = Random.Range(playerTransform.position.x - rangeX, playerTransform.position.x + rangeX);
        float randomZ = Random.Range(playerTransform.position.z - rangeZ, playerTransform.position.z + rangeZ);

        return new Vector3(randomX, playerTransform.position.y, randomZ);
    }

    IEnumerator FallIcicle(GameObject icicle)
    {
        // 落下までの遅延
        yield return new WaitForSeconds(fallDelay);

        // 氷柱を落下させる
        Rigidbody rb = icicle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 一定時間後に氷柱をリセットして非表示
        yield return new WaitForSeconds(3.0f);
        rb.isKinematic = true;
        icicle.SetActive(false); // 氷柱を非表示にして再利用可能にする
    }

    // エリアの範囲を視覚的に表示（ギズモ描画用）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(zoneCenter + transform.position, zoneSize);
    }
}
