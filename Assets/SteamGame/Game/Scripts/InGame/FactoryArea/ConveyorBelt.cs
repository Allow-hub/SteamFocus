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

        private void OnCollisionStay(Collision collision)
        {
            // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.MovePosition( rb.position + moveDirection.normalized * speed * Time.deltaTime);
            }
        }
    }
}
