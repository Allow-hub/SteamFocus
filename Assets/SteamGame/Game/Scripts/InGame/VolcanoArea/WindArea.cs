using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class WindArea : MonoBehaviour
    {
        public Vector3 windDirection = new Vector3(1f, 0f, 0f);  // 風の方向
        public float windStrength = 10f;  // 風の強さ
        public float arrowLength = 10f;    // 矢印の長さ
        public float arrowHeadLength = 2f; // 矢印の先端部分の長さ
        public float arrowHeadAngle = 150; // 矢印の先端部分の角度

        // 範囲内のオブジェクトに力を加える
        void OnTriggerStay(Collider other)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log(other.gameObject);
                Vector3 windForce = windDirection.normalized * windStrength;
                rb.AddForce(windForce);
            }
        }

        // シーンビューで風の方向を矢印で表示する
        void OnDrawGizmos()
        {
            // 風の向きを矢印で描く
            Gizmos.color = Color.blue;  // 矢印の色を青に設定

            // 矢印の始点（オブジェクトの位置）
            Vector3 startPosition = transform.position;
            // 矢印の終点（風の方向を伸ばした位置）
            Vector3 endPosition = startPosition + windDirection.normalized * arrowLength;

            // 風の向きの直線部分を描画
            Gizmos.DrawLine(startPosition, endPosition);

            // 矢印の先端を描く
            Vector3 rightArrowHead = endPosition + Quaternion.Euler(0, 0, arrowHeadAngle) * windDirection.normalized * arrowHeadLength;
            Vector3 leftArrowHead = endPosition + Quaternion.Euler(0, 0, -arrowHeadAngle) * windDirection.normalized * arrowHeadLength;

            Gizmos.DrawLine(endPosition, rightArrowHead);  // 右の矢印先端
            Gizmos.DrawLine(endPosition, leftArrowHead);   // 左の矢印先端
        }
    }
}
