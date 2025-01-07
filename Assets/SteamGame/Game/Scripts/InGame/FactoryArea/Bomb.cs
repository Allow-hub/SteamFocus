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
        [SerializeField] private GameObject effectObj, bombObj;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed = 360f; // 回転速度（度/秒）
        [SerializeField]
        private Vector3 rotateDirection;
        [SerializeField] private Rigidbody rb;


        private void OnEnable()
        {
            effectObj.SetActive(false);
            bombObj.SetActive(true);
            rb.velocity = moveDirection.normalized * moveSpeed;
        }
        private void Update()
        {
            // X軸で回転を続ける
            transform.Rotate(rotateDirection.normalized * rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 衝突したオブジェクトが "Ball" タグを持っているか確認
            if (collision.gameObject.CompareTag("Ball"))
            {
                if (SeManager.I != null)
                    SeManager.I.PlaySe(6, 1);
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null && explosionCenter != null)
                {
                    bombObj.SetActive(false);
                    effectObj.SetActive(true);
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

        public void SetProperty(Vector3 moveDir, Vector3 rotateDir, float mSpeed, float rSpeed)
        {
            moveDirection = moveDir;
            rotateDirection = rotateDir;
            moveSpeed = mSpeed;
            rotationSpeed = rSpeed;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (explosionCenter == null) return;

            // Explosion範囲を示すギズモを描画
            Gizmos.color = Color.red; // 赤色で表示
            Gizmos.DrawWireSphere(explosionCenter.position, explosionRadius); // 爆発範囲を球形で表示

            // diractionの方向に線を描画
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + moveDirection.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
#endif
    }
}