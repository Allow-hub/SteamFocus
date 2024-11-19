using UnityEngine;
using UnityEngine.InputSystem;

namespace TechC
{
    public class ReceiveNotification : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;  // プレイヤーのプレハブを指定
        [SerializeField] private GameObject canvasObj;
        [SerializeField] 
        private Transform createPos;  // プレイヤーをインスタンス化する位置
        private PlayerInputManager playerInputManager;

        // Start is called before the first frame update
        void Awake()
        {
            LogConnectedDevices();

            // PlayerInputManagerのインスタンスを取得
            playerInputManager = FindObjectOfType<PlayerInputManager>();

            if (playerInputManager != null)
            {
                // プレイヤーが参加したときに呼ばれるイベントをリスン
                playerInputManager.onPlayerJoined += OnPlayerJoined;
                playerInputManager.onPlayerLeft += OnPlayerLeft;
            }

            // 初期状態でプレイヤーをインスタンス化
            InstantiatePlayerAtPosition();
        }

        // プレイヤーをcreatePosの位置にインスタンス化する
        private void InstantiatePlayerAtPosition()
        {
            // createPosの位置にプレイヤーをインスタンス化
            GameObject playerInstance = PlayerInput.Instantiate(playerPrefab, createPos.position, Quaternion.identity);

            // プレイヤーのサイズを設定（例えば、5, 5, 5にする）
            playerInstance.transform.localScale = new Vector3(5f, 5f, 5f);
        }

        // プレイヤーがゲームに参加したときの処理
        public void OnPlayerJoined(PlayerInput playerInput)
        {
            Debug.Log("プレイヤーが参加しました");

            // 参加したプレイヤーの操作デバイスをログに出力
            foreach (var device in playerInput.devices)
            {
                Debug.Log("操作デバイス: " + device);
            }
        }

        // プレイヤーがゲームから退出したときの処理
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log($"プレイヤー#{playerInput.user.index}が退室しました");
        }

        // プレイヤーが参加するイベントを手動で発火させる
        public void InstantiateCharacter()
        {
            GameManager.I.ChangeTutorialState();
            InstantiatePlayerAtPosition();
            canvasObj.SetActive(false);

        }
        // 現在接続されているデバイスをログに出力するメソッド
        private void LogConnectedDevices()
        {
            var devices = InputSystem.devices;  // 接続されているすべてのデバイスを取得

            if (devices.Count == 0)
            {
                Debug.Log("現在接続されているデバイスはありません");
            }
            else
            {
                foreach (var device in devices)
                {
                    // device.name を使ってデバイスの名前を出力
                    Debug.Log($"接続されているデバイス: {device.name} (タイプ: {device.GetType().Name})");
                }
            }
        }
        // OnDestroyでイベントリスナーを解除する（メモリリーク防止）
        private void OnDestroy()
        {
            if (playerInputManager != null)
            {
                // イベントリスナーを解除
                playerInputManager.onPlayerJoined -= OnPlayerJoined;
                playerInputManager.onPlayerLeft -= OnPlayerLeft;
            }
        }
    }
}
