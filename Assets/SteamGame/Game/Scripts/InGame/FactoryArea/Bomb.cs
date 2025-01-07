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
        [SerializeField] private GameObject effectObj, bombObj;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed = 360f; // ��]���x�i�x/�b�j
        [SerializeField]
        private Vector3 rotateDirection;
        [SerializeField] private Rigidbody rb;


        private void OnEnable()
        {
            effectObj.SetActive(false);
            bombObj.SetActive(true);
            rb.velocity = moveDirection.normalized * moveSpeed;
        }
        private void Update()
        {
            // X���ŉ�]�𑱂���
            transform.Rotate(rotateDirection.normalized * rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // �Փ˂����I�u�W�F�N�g�� "Ball" �^�O�������Ă��邩�m�F
            if (collision.gameObject.CompareTag("Ball"))
            {
                if (SeManager.I != null)
                    SeManager.I.PlaySe(6, 1);
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null && explosionCenter != null)
                {
                    bombObj.SetActive(false);
                    effectObj.SetActive(true);
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

        public void SetProperty(Vector3 moveDir, Vector3 rotateDir, float mSpeed, float rSpeed)
        {
            moveDirection = moveDir;
            rotateDirection = rotateDir;
            moveSpeed = mSpeed;
            rotationSpeed = rSpeed;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (explosionCenter == null) return;

            // Explosion�͈͂������M�Y����`��
            Gizmos.color = Color.red; // �ԐF�ŕ\��
            Gizmos.DrawWireSphere(explosionCenter.position, explosionRadius); // �����͈͂����`�ŕ\��

            // diraction�̕����ɐ���`��
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + moveDirection.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
#endif
    }
}