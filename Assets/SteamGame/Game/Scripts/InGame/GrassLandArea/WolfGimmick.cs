using UnityEngine;
using UnityEngine.AI;


namespace TechC
{
    public class WolfGimmick : MonoBehaviour
    {
        [SerializeField] Transform Ball;  // ボールのオブジェクト
        [SerializeField] string TerritoryTag = "Territory";  // 縄張りのタグ
        [SerializeField] float TerritoryRadius = 5f;  // 縄張りの半径

        private Vector3 originalPosition;  // 狼の元々の位置
        private NavMeshAgent Nav;  // ナビメッシュエージェント

        void Start()
        {
            Nav = GetComponent<NavMeshAgent>();
            originalPosition = transform.position;  // 最初の位置を保存
        }

        void Update()
        {
            // ボールが縄張り内にいるかをチェック
            if (Vector3.Distance(transform.position, Ball.position) <= TerritoryRadius)
            {
                // ボールが縄張り内にいる場合、ボールを追いかける
                if (Ball.CompareTag("Ball"))
                {
                    Nav.SetDestination(Ball.position);
                }
            }
            else
            {
                // ボールが縄張り外に出た場合、元の位置に戻る
                Nav.SetDestination(originalPosition);
            }
        }

        // Optional: トリガーでボールの出入りを検出
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                // ボールが縄張りに入ったときに追いかける
                Nav.SetDestination(Ball.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                // ボールが縄張りから出たときに元の位置に戻る
                Nav.SetDestination(originalPosition);
            }
        }
    }
}
