using Photon.Pun;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class AvatarNameDisplay : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            var nameLabel = GetComponent<TextMeshPro>();
            // �v���C���[���ƃv���C���[ID��\������
            nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        }
    }
}
