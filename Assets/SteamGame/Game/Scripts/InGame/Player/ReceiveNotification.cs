using UnityEngine;
using UnityEngine.InputSystem;

namespace TechC
{
    public class ReceiveNotification : MonoBehaviour
    {
        // プレイヤー入室時に受け取る通知
        public void OnPlayerJoined(PlayerInput playerInput)
        {
            print($"プレイヤー#{playerInput.user.index}が入室！");
        }

        // プレイヤー退室時に受け取る通知
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            print($"プレイヤー#{playerInput.user.index}が退室！");
        }
    }
}
