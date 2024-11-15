using UnityEngine;

namespace TechC
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody rb;

        //[SerializeField] private float gravityMultiplier = 5f; // 重力を何倍にするか
        //[SerializeField] private float groundCheckDistance = 0.1f; // 地面との距離
        //[SerializeField] private float gravityApplyInterval = 0.5f; // 重力強化の間隔

        //private bool isHeavy = true; // 初期状態では空中にいると仮定
        //private float gravityTimer = 0f; // 重力強化のタイマー

        private Vector3 direction;
        [SerializeField] private Vector2 rotSpeedRange = new Vector2(1, 3);
        private float rotationSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();  // Rigidbodyの参照を取得
        }

        // オブジェクトが有効になったときの初期設定
        private void OnEnable()
        {
            direction = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            rotationSpeed = Random.Range(rotSpeedRange.x, rotSpeedRange.y);
        }

        // 毎フレーム呼び出され、オブジェクトをランダムな方向に回転させる
        private void Update()
        {
            transform.Rotate(direction.x * Time.deltaTime * rotationSpeed,
                             direction.y * Time.deltaTime * rotationSpeed,
                             direction.z * Time.deltaTime * rotationSpeed);
        }

    }
}
