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

        private void OnCollisionEnter(Collision collision)
        {
            // �Փ˂����I�u�W�F�N�g�� "Ball" �^�O�������Ă��邩�m�F
            if (collision.gameObject.CompareTag("Ball"))
            {
                // �I�u�W�F�N�g���x���g�R���x�A�̂悤�Ɉړ�������
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.gameObject.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }
    }
}
