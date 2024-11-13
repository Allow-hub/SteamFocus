using UnityEngine;

namespace TechC
{
    public class Bomb : MonoBehaviour
    {
        [Header("���e")]
        [Multiline(5)]
        [SerializeField] private string explain;
        [Header("�����̐ݒ�")]
        [SerializeField] private float explosionForce = 1000f; // ������
        [SerializeField] private float explosionRadius = 5f; // �����͈�
        [SerializeField] private float upwardsModifier = 1f; // ������̗͂̒���

        private void OnCollisionEnter(Collision collision)
        {
            // �Փ˂����I�u�W�F�N�g�� "Ball" �^�O�������Ă��邩�m�F
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // �Փ˂����ʒu����AddExplosionForce�Ŕ���������
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                }
            }
        }
    }
}
