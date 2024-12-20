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
            // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
            PhotonNetwork.ConnectUsingSettings();
        }

        // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
        public override void OnConnectedToMaster()
        {
            // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
            //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
            PhotonNetwork.JoinLobby();

        }

        // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
        public override void OnJoinedRoom()
        {
            // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            var position =pos.position;
            var obj =  PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            if (avatarParent == null) return;
            obj.transform.SetParent(avatarParent);
            GameManager.I.AddListPlayer(obj);
        }
    }
}
