using System.Collections;
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
        [SerializeField] private Transform shotPos;
        [SerializeField] private GameObject[] debris; // ���ł�����prefab�i�v�[���Ώہj
        [SerializeField] private float maxInterval, minInterval;
        [SerializeField] private GameObject player,explosionObj;
        [SerializeField] private float explosionDuration = 3f;
 
        [SerializeField] private Vector2 xRange,zRange;  // Y���̃����_���͈́i�ŏ����x�A�ő卂�x�j
        //[SerializeField] private Vector2 forceRange; // ��΂��͈͂̔́i�ŏ��́A�ő�́j
        [SerializeField] private Vector2 shotCountRange;

        [SerializeField] private bool canShake = true;
        [Header("�v���C���[���͂̔��a")]
        [SerializeField] private float throwAngle = 45;

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool; // ObjectPool���Q��
        [SerializeField] private ChaseCamera chaseCamera;
        private float elapsedTime = 0;
        private float currentInterval;
        private const float delay = 0.3f;
        private void Awake()
        {
            elapsedTime = 0;
            currentInterval = Random.Range(minInterval, maxInterval);
            explosionObj.SetActive(false);
        }

        void Start()
        {
            GameManager.I.ChangeVolcanoState();
        }

        private void Update()
        {
            if (GameManager.I == null) return;
            if (!GameManager.I.IsVolcanoArea()) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > currentInterval)
            {
                elapsedTime = 0;
                currentInterval = Random.Range(minInterval, maxInterval);
                StartCoroutine(ShotDelay(delay,Random.Range((int)shotCountRange.x, (int)shotCountRange.y)));
            }
        }

        private IEnumerator ShotDelay(float delay,int count)
        {
            for (int i = 0; i < count; i++)
            {
                Shot();
                yield return new WaitForSeconds(delay);
            }
        }
        /// <summary>
        /// �v���C���[�̎��͂̔��a3�ȓ��Ƀ����_���Ɋ���΂�
        /// </summary>
        private void Shot()
        {
            StartCoroutine(PlayExplosionEffect());
            GameObject selectedDebris = debris[Random.Range(0, debris.Length)];
            GameObject newDebris = objectPool.GetObject(selectedDebris);

            newDebris.transform.position =shotPos.transform.position;
            newDebris.transform.rotation = Quaternion.identity;

            Rigidbody rb = newDebris.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomPos = new Vector3(
                        player.transform.position.x+Random.Range(xRange.x,xRange.y),
                        player.transform.position.y,
                        player.transform.position.z+Random.Range(zRange.x, zRange.y)
                );

                Vector3 targetPos = CalculateVelocity(transform.position, randomPos, throwAngle);
                rb.AddForce(targetPos * rb.mass, ForceMode.Impulse);
            }
        }
        /// <summary>
        /// �W�I�ɖ�������ˏo���x�̌v�Z
        /// </summary>
        /// <param name="pointA">�ˏo�J�n���W</param>
        /// <param name="pointB">�W�I�̍��W</param>
        /// <returns>�ˏo���x</returns>
        private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
        {
            // �ˏo�p�����W�A���ɕϊ�
            float rad = angle * Mathf.PI / 180;

            // ���������̋���x
            float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

            // ���������̋���y
            float y = pointA.y - pointB.y;

            // �Ε����˂̌����������x�ɂ��ĉ���
            float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

            if (float.IsNaN(speed))
            {
                // �����𖞂����������Z�o�ł��Ȃ����Vector3.zero��Ԃ�
                return Vector3.zero;
            }
            else
            {
                return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
            }
        }


        private IEnumerator PlayExplosionEffect()
        {
            explosionObj.SetActive(true);
            if (canShake)
                chaseCamera.TriggerShake();
            yield return new WaitForSeconds(explosionDuration);
            explosionObj.SetActive(false);
        }
    }
}
