using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace TechC
{
    public class NetWorkManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform pos;
        [SerializeField] private Transform avatarParent;

        private void Awake()
        {
            PhotonNetwork.NickName = "Player";
            // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        }

        // マスターサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnConnectedToMaster()
        {
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
            PhotonNetwork.JoinLobby();

        }

        // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
            var position =pos.position;
            var obj =  PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            if (avatarParent == null) return;
            obj.transform.SetParent(avatarParent);
            GameManager.I.AddListPlayer(obj);
        }
    }
}
