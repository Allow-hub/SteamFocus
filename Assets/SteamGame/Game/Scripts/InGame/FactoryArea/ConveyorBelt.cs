using UnityEngine;

namespace TechC
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("�x���g�R���x�A")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("�ړ������Ƒ��x")]
        [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0); // �O���[�o���ϐ��ňړ��������`
        [SerializeField] private float speed = 2f;

        [Header("���C���[�ݒ�")]
        [SerializeField] private LayerMask layerMask;  // �Փ˂��郌�C���[���w��

        private void OnCollisionEnter(Collision collision)
        {
            // �Փ˂����I�u�W�F�N�g�̃��C���[���w�肳�ꂽ���C���[���m�F
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                // �I�u�W�F�N�g���x���g�R���x�A�̂悤�Ɉړ�������
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                Debug.Log("A");
                Debug.Log(collision.gameObject);
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }
    }
}
