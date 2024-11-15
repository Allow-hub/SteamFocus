using UnityEngine;

namespace TechC
{
    public class Bomb : MonoBehaviour
    {
        [Header("爆弾")]
        [Multiline(5)]
        [SerializeField] private string explain;
        [Header("爆発の設定")]
        [SerializeField] private float explosionForce = 1000f; // 爆発力
        [SerializeField] private float explosionRadius = 5f; // 爆発範囲
        [SerializeField] private float upwardsModifier = 1f; // 上方向の力の調整

        private void OnCollisionEnter(Collision collision)
        {
            // 衝突したオブジェクトが "Ball" タグを持っているか確認
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 衝突した位置からAddExplosionForceで反発させる
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                }
            }
        }
    }
}
