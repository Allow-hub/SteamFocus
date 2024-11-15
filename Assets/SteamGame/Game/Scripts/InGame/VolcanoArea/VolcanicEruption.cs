using System.Collections;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// 火砕流、ランダムに飛んでくる
    /// </summary>
    public class VolcanicEruption : MonoBehaviour
    {
        [Header("火砕流")]
        [Multiline(5)]
        [SerializeField] private string explain;
        [SerializeField] private Transform shotPos;
        [SerializeField] private GameObject[] debris; // 飛んでくる岩のprefab（プール対象）
        [SerializeField] private float maxInterval, minInterval;
        [SerializeField] private GameObject player,explosionObj;
        [SerializeField] private float explosionDuration = 3f;
 
        [SerializeField] private Vector2 xRange,zRange;  // Y軸のランダム範囲（最小高度、最大高度）
        //[SerializeField] private Vector2 forceRange; // 飛ばす力の範囲（最小力、最大力）
        [SerializeField] private Vector2 shotCountRange;

        [SerializeField] private bool canShake = true;
        [Header("プレイヤー周囲の半径")]
        [SerializeField] private float throwAngle = 45;

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool; // ObjectPoolを参照
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
        /// プレイヤーの周囲の半径3以内にランダムに岩を飛ばす
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
        /// 標的に命中する射出速度の計算
        /// </summary>
        /// <param name="pointA">射出開始座標</param>
        /// <param name="pointB">標的の座標</param>
        /// <returns>射出速度</returns>
        private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
        {
            // 射出角をラジアンに変換
            float rad = angle * Mathf.PI / 180;

            // 水平方向の距離x
            float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

            // 垂直方向の距離y
            float y = pointA.y - pointB.y;

            // 斜方投射の公式を初速度について解く
            float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

            if (float.IsNaN(speed))
            {
                // 条件を満たす初速を算出できなければVector3.zeroを返す
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
