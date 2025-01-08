using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [Header("Rock Settings")]
    [SerializeField]private List<GameObject> rockObjects;  // インスペクターで設定する岩オブジェクトのリスト
    [SerializeField] private float fallInterval = 2.0f;    // 岩が落ちる間隔
    [SerializeField] private float spawnHeight = 10.0f;    // 岩が生成される高さ
    [SerializeField] private float fallDelay = 1.5f;       // 岩が落下を開始するまでの遅延時間
    [SerializeField] private Vector3 spawnAreaCenter;      // 岩が落ちるエリアの中心
    [SerializeField] private Vector3 spawnAreaSize;        // 岩が落ちるエリアのサイズ

    private int nextRockIndex = 0;       // 次に使用する岩のインデックス
    [SerializeField] private GameObject spawnAreaObject; // スポーンエリア用オブジェクト

    void Start()
    {
        if (spawnAreaObject != null)
        {
            // スポーンエリアの中心とサイズを取得
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

            // 使用する岩を取得
            GameObject rock = rockObjects[nextRockIndex];
            nextRockIndex = (nextRockIndex + 1) % rockObjects.Count;

            // ランダムな位置に岩を生成
            Vector3 spawnPosition = GetRandomSpawnPosition();
            spawnPosition.y += spawnHeight; // Y方向を調整
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
        yield return new WaitForSeconds(4.5f);
        rb.isKinematic = true;
        rock.SetActive(false); // 岩を非表示にして再利用可能にする
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // スポーンエリア内でランダムな位置を生成
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float z = Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);

        return new Vector3(x, spawnAreaCenter.y, z);
    }
}
