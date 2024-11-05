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
        [SerializeField] private GameObject[] debris; // 飛んでくる岩のprefab（プール対象）
        [SerializeField] private float maxInterval, minInterval;
        [SerializeField] private GameObject player;

        [SerializeField] private Vector2 xRange, zRange;
        [SerializeField] private Vector2 yRange;  // Y軸のランダム範囲（最小高度、最大高度）
        float radius = 5f;  // 任意の半径

        [Header("飛ばす力の範囲")]
        [SerializeField] private Vector2 forceRange; // 飛ばす力の範囲（最小力、最大力）

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool; // ObjectPoolを参照

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
            // 火山エリアではないときリターン
            if (!GameManager.I.IsVolcanoArea()) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > currentInterval)
            {
                elapsedTime = 0;
                currentInterval = Random.Range(minInterval, maxInterval);
                Shot(); // Intervalが過ぎたら岩を飛ばす
            }
        }

        /// <summary>
        /// 岩をプレイヤーの周囲にランダムに飛ばす
        /// </summary>
        private void Shot()
        {
            // 飛ばす岩をランダムに選ぶ
            GameObject selectedDebris = debris[Random.Range(0, debris.Length)];

            // ObjectPool から岩のオブジェクトを取得
            GameObject newDebris = objectPool.GetObject(selectedDebris);

            // 新しく取得した岩を適切な位置に配置
            newDebris.transform.position = transform.position;  // このスクリプトがアタッチされているオブジェクト（火山）の位置に岩を生成
            newDebris.transform.rotation = Quaternion.identity;

            // Rigidbodyを取得して、岩に力を加えて飛ばす
            Rigidbody rb = newDebris.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Random.insideUnitCircle でランダムな位置を決定（2D平面上）
                Vector2 randomPos = Random.insideUnitCircle * radius;

                // プレイヤーの位置を基準にランダムな位置を計算（Y軸はランダム）
                Vector3 randomDirection = new Vector3(randomPos.x, Random.Range(yRange.x, yRange.y), randomPos.y) + player.transform.position;

                // プレイヤーの位置からランダムな位置への方向
                Vector3 direction = (randomDirection - player.transform.position).normalized;

                // 飛ばす力の大きさ（調整可能）
                float force = Random.Range(forceRange.x, forceRange.y); // forceRangeを使って力をランダムに決定

                // 力を加える
                rb.AddForce(direction * force * 100, ForceMode.Impulse); // 飛ばす方向と力を加えます
            }
        }
    }
}
