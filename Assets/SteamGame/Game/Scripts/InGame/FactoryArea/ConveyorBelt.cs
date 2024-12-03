using UnityEngine;

namespace TechC
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("ベルトコンベア")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("移動方向と速度")]
        [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0); // グローバル変数で移動方向を定義
        [SerializeField] private float speed = 2f;

        [Header("レイヤー設定")]
        [SerializeField] private LayerMask layerMask;  // 衝突するレイヤーを指定

        private void OnCollisionStay(Collision collision)
        {
            // 衝突が続いている間もオブジェクトを動かし続ける
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.MovePosition( rb.position + moveDirection.normalized * speed * Time.deltaTime);
            }
        }
    }
}
