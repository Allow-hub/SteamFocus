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
        [SerializeField] private string avatarParentName;
        private GameObject avatarParent;

        [Header("レイヤー設定")]
        [SerializeField] private LayerMask layerMask;  // 衝突するレイヤーを指定
        [SerializeField] private bool isDrawing;
        const int gizmosLenge = 30;

        private void OnValidate()
        {
            avatarParent = GameObject.Find(avatarParentName);
        }

     
        private void OnTriggerStay(Collider col)
        {
            // 衝突が続いている間もオブジェクトを動かし続ける
            if (((1 << col.gameObject.layer) & layerMask) != 0)
            {
                var rb = col.gameObject.GetComponent<Rigidbody>();
                rb.MovePosition( rb.position + moveDirection.normalized * speed * Time.deltaTime);
            }
        }
        private void OnDrawGizmos()
        {
            if (!isDrawing) return;
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + moveDirection.normalized * gizmosLenge;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
