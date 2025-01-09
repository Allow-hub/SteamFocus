using System.Collections;
using System.Collections.Generic;
using TechC;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    public GameObject snowballPrefab; // 雪玉のPrefab
    public Transform snowballSpawnPoint; // 雪玉の生成位置
    public float snowballForce = 20f; // 雪玉の初速度
    public float autoThrowInterval = 3f; // 雪玉を自動で投げる間隔
    public Vector3 throwDirection = new Vector3(0, 1, 1); // 雪玉を投げる方向（斜め上）

    public float snowballDestroyTime = 10f; // 雪玉が消えるまでの時間

    private void Start()
    {
        // 雪玉を自動で投げるコルーチンを開始
        StartCoroutine(AutoThrowSnowball());
    }

    private IEnumerator AutoThrowSnowball()
    {
        while (true)
        {
            ThrowSnowball(); // 雪玉を投げる
            yield return new WaitForSeconds(autoThrowInterval); // 指定間隔で待機
        }
    }

    private void ThrowSnowball()
    {
        // 雪玉を生成
        GameObject snowball = objectPool.GetObject(snowballPrefab);
        snowball.transform.position = snowballSpawnPoint.position;
        //snowball.transform.rotation =Quaternion.identity;

        // 雪玉の向きと初速度を設定
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 投げる方向を正規化して力を加える
            Vector3 normalizedDirection = throwDirection.normalized; // ベクトルを正規化
            rb.AddForce(normalizedDirection * snowballForce, ForceMode.Impulse); // 雪玉を投げる
        }

        // 一定時間後に雪玉を削除
        //Destroy(snowball, snowballDestroyTime);
    }
}