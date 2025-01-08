using System.Collections;
using System.Collections.Generic;
using TechC;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;    // 爆発の範囲
    [SerializeField] private float explosionForce = 10; // 爆発の力

    [SerializeField] private float upwardsModifier = 5; // 爆発の力
    [SerializeField] private GameObject mineObj;
    [SerializeField] private GameObject explosionEffect;   // 爆発のエフェクト（プレハブ）
    [SerializeField] private float cameraShakeDuration = 0.5f; // カメラシェイクの時間
    [SerializeField] private float cameraShakeMagnitude = 0.2f; // カメラシェイクの強度
    [SerializeField] private float popInterval = 10f;
    [SerializeField] private bool canDraw = false;

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Explode(other.gameObject); // プレイヤーが触れると爆発
        }
    }

    void Explode(GameObject obj)
    {
        if (SeManager.I != null)
            SeManager.I.PlaySe(6, 1); //爆発の音
        Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();

        if (rb != null && transform.position != null)
        {
            // 爆発中心からAddExplosionForceで反発させる
            rb.AddExplosionForce(
                explosionForce,
               transform.position,
                explosionRadius,
                upwardsModifier,
                ForceMode.Impulse
            );
        }

        // カメラシェイクを実行
        //CameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude);
        StartCoroutine(ResetObj());

    }

    private IEnumerator ResetObj()
    {
        mineObj.SetActive(false);
        explosionEffect.SetActive(true);
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(popInterval);
        mineObj.SetActive(true);
        explosionEffect.SetActive(false);
        capsuleCollider.enabled = true;

    }

    private void OnDrawGizmos()
    {
        if (!canDraw) return;
        // Explosion範囲を示すギズモを描画
        Gizmos.color = Color.red; // 赤色で表示
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // 爆発範囲を球形で表示
    }
}
