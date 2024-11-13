using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    public GameObject snowballPrefab; // 雪玉のPrefab
    public Transform snowballSpawnPoint; // 雪玉の生成位置
    public float snowballForce = 10f; // 雪玉を転がす初速度
    public float autoRollInterval = 5f; // 雪玉を転がす間隔
    public float snowballDestroyTime = 10f; // 雪玉が消えるまでの時間

    private void Start()
    {
        // 雪玉を自動で転がすコルーチンを開始
        StartCoroutine(AutoRollSnowball());
    }

    private IEnumerator AutoRollSnowball()
    {
        while (true)
        {
            RollSnowball(); // 雪玉を転がす
            yield return new WaitForSeconds(autoRollInterval); // 指定時間待機
        }
    }

    private void RollSnowball()
    {
        // 雪玉を生成
        GameObject snowball = Instantiate(snowballPrefab, snowballSpawnPoint.position, Quaternion.identity);

        // 雪玉の斜面方向に初速度を与える
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 斜面の向きを取得
            Vector3 slopeDirection = GetSlopeDirection(snowballSpawnPoint.position);
            rb.AddForce(slopeDirection * snowballForce, ForceMode.Impulse); // 雪玉に初速度を与える
        }

        // 一定時間後に雪玉を削除
        Destroy(snowball, snowballDestroyTime);
    }

    private Vector3 GetSlopeDirection(Vector3 position)
    {
        // 斜面の法線を取得
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 5f)) // 地面に向けたRaycastで斜面の法線を取得
        {
            // 法線から斜面方向を計算
            Vector3 slopeDirection = Vector3.Cross(hit.normal, Vector3.Cross(Vector3.down, hit.normal));
            return slopeDirection.normalized; // 斜面に沿った進行方向を返す
        }
        return Vector3.forward; // 万が一取得できなかった場合のデフォルト方向
    }
}
