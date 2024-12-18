using UnityEngine;

namespace TechC
{
    public class Bomb : MonoBehaviour
    {
        [Header("爆弾")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("爆発の設定")]
        [SerializeField] private Transform explosionCenter; // 爆発の中心を指定
        [SerializeField] private float explosionForce = 1000f; // 爆発力
        [SerializeField] private float explosionRadius = 5f; // 爆発範囲
        [SerializeField] private float upwardsModifier = 1f; // 上方向の力の調整

        private void OnCollisionEnter(Collision collision)
        {
            // 衝突したオブジェクトが "Ball" タグを持っているか確認
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null && explosionCenter != null)
                {
                    // 爆発中心からAddExplosionForceで反発させる
                    rb.AddExplosionForce(
                        explosionForce,
                        explosionCenter.position, // 設定された中心の位置を使用
                        explosionRadius,
                        upwardsModifier,
                        ForceMode.Impulse
                    );
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (explosionCenter == null) return;

            // Explosion範囲を示すギズモを描画
            Gizmos.color = Color.red; // 赤色で表示
            Gizmos.DrawWireSphere(explosionCenter.position, explosionRadius); // 爆発範囲を球形で表示
        }
#endif
    }
}
