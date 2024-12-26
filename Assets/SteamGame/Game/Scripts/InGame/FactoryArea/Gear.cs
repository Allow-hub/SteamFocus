using UnityEngine;

namespace TechC
{
    public class Gear : MonoBehaviour
    {
        [Header("歯車")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private Vector3 rotateDirection; // 回転方向と速度を設定
        [SerializeField] private float speed = 5;

        private void Update()
        {
            // 毎フレーム回転を適用
            transform.Rotate(rotateDirection * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                other.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                other.transform.parent = null;
            }
        }
    }
}
