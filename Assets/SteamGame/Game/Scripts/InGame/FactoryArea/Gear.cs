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
            transform.Rotate(rotateDirection*speed * Time.deltaTime);
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Ball"))
        //    {
        //        collision.gameObject.transform.parent = transform;
        //    }
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Ball"))
        //    {
        //        collision.gameObject.transform.parent = null;
        //    }
        //}
    }
}
