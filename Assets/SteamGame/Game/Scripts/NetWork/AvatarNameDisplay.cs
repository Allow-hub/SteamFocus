using Photon.Pun;
using TMPro;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof (TMP_Text))]
    public class AvatarNameDisplay : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            var nameLabel = GetComponent<TMP_Text>();
            
            // �v���C���[���ƃv���C���[ID��\������
            nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        }
    }
}
