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

        private void OnCollisionEnter(Collision collision)
        {
            // 衝突したオブジェクトが "Ball" タグを持っているか確認
            if (collision.gameObject.CompareTag("Ball"))
            {
                // オブジェクトをベルトコンベアのように移動させる
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            // 衝突が続いている間もオブジェクトを動かし続ける
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }
    }
}
