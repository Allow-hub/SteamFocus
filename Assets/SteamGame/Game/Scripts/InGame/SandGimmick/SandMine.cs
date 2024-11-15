using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    public float explosionRadius = 5f;   // 爆発の範囲
    public float explosionForce = 10f;   // 爆発の力

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Explode();  // プレイヤーが触れると爆発
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);  // 爆風で吹き飛ばす
            }
        }

        Destroy(gameObject);  // 地雷を消す
    }
}
