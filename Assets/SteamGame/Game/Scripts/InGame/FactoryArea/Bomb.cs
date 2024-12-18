using UnityEngine;

namespace TechC
{
    public class Bomb : MonoBehaviour
    {
        [Header("���e")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("�����̐ݒ�")]
        [SerializeField] private Transform explosionCenter; // �����̒��S���w��
        [SerializeField] private float explosionForce = 1000f; // ������
        [SerializeField] private float explosionRadius = 5f; // �����͈�
        [SerializeField] private float upwardsModifier = 1f; // ������̗͂̒���

        private void OnCollisionEnter(Collision collision)
        {
            // �Փ˂����I�u�W�F�N�g�� "Ball" �^�O�������Ă��邩�m�F
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null && explosionCenter != null)
                {
                    // �������S����AddExplosionForce�Ŕ���������
                    rb.AddExplosionForce(
                        explosionForce,
                        explosionCenter.position, // �ݒ肳�ꂽ���S�̈ʒu���g�p
                        explosionRadius,
                        upwardsModifier,
                        ForceMode.Impulse
                    );
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (explosionCenter == null) return;

            // Explosion�͈͂������M�Y����`��
            Gizmos.color = Color.red; // �ԐF�ŕ\��
            Gizmos.DrawWireSphere(explosionCenter.position, explosionRadius); // �����͈͂����`�ŕ\��
        }
#endif
    }
}
