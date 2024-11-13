using UnityEngine;

namespace TechC
{
    public class SteamVent : MonoBehaviour
    {
        [Header("蒸気")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("蒸気の設定")]
        [SerializeField] private float launchForce = 10f; // 上方向に飛ばす力
        [SerializeField] private float steamInterval = 2f; // 噴射のタイミング間隔

        private bool isPlayerInArea = false;
        private Rigidbody playerRb;
        private float timer;

        private void Update()
        {
            // 噴射タイミングをカウント
            timer += Time.deltaTime;

            // 一定時間ごとに噴射を行う
            if (timer >= steamInterval)
            {
                // プレイヤーがエリア内にいる場合、上に飛ばす
                if (isPlayerInArea && playerRb != null)
                {
                    LaunchPlayer();
                }
                timer = 0f; // タイマーをリセット
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                playerRb = other.gameObject.transform.parent.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    isPlayerInArea = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // プレイヤーがエリアから出た時、フラグをリセット
            if (other.CompareTag("Ball"))
            {
                isPlayerInArea = false;
                playerRb = null;
            }
        }

        private void LaunchPlayer()
        {
            // プレイヤーのRigidbodyに上向きの力を加える
            playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
    }
}
