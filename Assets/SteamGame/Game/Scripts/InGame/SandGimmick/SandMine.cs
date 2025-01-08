using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    public float explosionRadius = 5f;    // 爆発の範囲
    public float explosionForce = 1000f; // 爆発の力
    public GameObject explosionEffect;   // 爆発のエフェクト（プレハブ）
    public AudioClip explosionSound;     // 爆発のサウンド
    public float cameraShakeDuration = 0.5f; // カメラシェイクの時間
    public float cameraShakeMagnitude = 0.2f; // カメラシェイクの強度

    private AudioSource audioSource;

    void Start()
    {
        // オーディオソースを設定
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Explode(); // プレイヤーが触れると爆発
        }
    }

    void Explode()
    {
        // 爆発エフェクトを生成
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 爆発音を再生
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // 爆発の影響を与える
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius); // 爆風で吹き飛ばす
            }
        }

        // カメラシェイクを実行
        CameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude);

        // 地雷を消す
        Destroy(gameObject);
    }
}
