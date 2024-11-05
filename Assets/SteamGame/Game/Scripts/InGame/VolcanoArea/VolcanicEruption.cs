using UnityEngine;

namespace TechC
{
    /// <summary>
    /// �΍ӗ��A�����_���ɔ��ł���
    /// </summary>
    public class VolcanicEruption : MonoBehaviour
    {
        [Header("�΍ӗ�")]
        [Multiline(5)]
        [SerializeField] private string explain;
        [SerializeField] private GameObject[] debris; // ���ł�����prefab�i�v�[���Ώہj
        [SerializeField] private float maxInterval, minInterval;
        [SerializeField] private GameObject player;

        [SerializeField] private Vector2 xRange, zRange;
        [SerializeField] private Vector2 yRange;  // Y���̃����_���͈́i�ŏ����x�A�ő卂�x�j
        float radius = 5f;  // �C�ӂ̔��a

        [Header("��΂��͈͂̔�")]
        [SerializeField] private Vector2 forceRange; // ��΂��͈͂̔́i�ŏ��́A�ő�́j

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool; // ObjectPool���Q��

        private float elapsedTime = 0;
        private float currentInterval;

        private void Awake()
        {
            elapsedTime = 0;
            currentInterval = Random.Range(minInterval, maxInterval);
        }

        void Start()
        {
            GameManager.I.ChangeVolcanoState();
        }

        private void Update()
        {
            if (GameManager.I == null) return;
            // �ΎR�G���A�ł͂Ȃ��Ƃ����^�[��
            if (!GameManager.I.IsVolcanoArea()) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > currentInterval)
            {
                elapsedTime = 0;
                currentInterval = Random.Range(minInterval, maxInterval);
                Shot(); // Interval���߂��������΂�
            }
        }

        /// <summary>
        /// ����v���C���[�̎��͂Ƀ����_���ɔ�΂�
        /// </summary>
        private void Shot()
        {
            // ��΂���������_���ɑI��
            GameObject selectedDebris = debris[Random.Range(0, debris.Length)];

            // ObjectPool �����̃I�u�W�F�N�g���擾
            GameObject newDebris = objectPool.GetObject(selectedDebris);

            // �V�����擾�������K�؂Ȉʒu�ɔz�u
            newDebris.transform.position = transform.position;  // ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�i�ΎR�j�̈ʒu�Ɋ�𐶐�
            newDebris.transform.rotation = Quaternion.identity;

            // Rigidbody���擾���āA��ɗ͂������Ĕ�΂�
            Rigidbody rb = newDebris.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Random.insideUnitCircle �Ń����_���Ȉʒu������i2D���ʏ�j
                Vector2 randomPos = Random.insideUnitCircle * radius;

                // �v���C���[�̈ʒu����Ƀ����_���Ȉʒu���v�Z�iY���̓����_���j
                Vector3 randomDirection = new Vector3(randomPos.x, Random.Range(yRange.x, yRange.y), randomPos.y) + player.transform.position;

                // �v���C���[�̈ʒu���烉���_���Ȉʒu�ւ̕���
                Vector3 direction = (randomDirection - player.transform.position).normalized;

                // ��΂��͂̑傫���i�����\�j
                float force = Random.Range(forceRange.x, forceRange.y); // forceRange���g���ė͂������_���Ɍ���

                // �͂�������
                rb.AddForce(direction * force * 100, ForceMode.Impulse); // ��΂������Ɨ͂������܂�
            }
        }
    }
}
