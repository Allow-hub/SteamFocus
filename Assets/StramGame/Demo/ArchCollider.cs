using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class ArchCollider : MonoBehaviour
    {
        public int segments = 10; // アーチのセグメント数
        public float radius = 5f; // アーチの半径
        public float height = 2f; // アーチの高さ
        public   float colliderSize = 0.2f, debugSize = 0.2f;
        public Vector3 rotation = new Vector3(0, 90, 0); // アーチの向きを制御するパラメータ
        public Vector3 centerOffset = new Vector3(1, 0, 0); // アーチの中心をずらすオフセット
        public bool showGizmos = true; // Gizmosの表示を制御するbool変数
        private List<GameObject> segmentColliders = new List<GameObject>();

        void Start()
        {
            CreateColliders();
        }

        public void CreateColliders()
        {
            // 既存のコライダーを削除
            foreach (var segment in segmentColliders)
            {
                Destroy(segment);
            }
            segmentColliders.Clear();

            Quaternion rotationQuaternion = Quaternion.Euler(rotation); // 回転をクォータニオンに変換

            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius + height, 0);

                // 回転を適用
                position = rotationQuaternion * position;

                // 中心位置のオフセットを加算
                position += centerOffset;

                GameObject colliderObj = new GameObject("SegmentCollider");
                colliderObj.transform.position = position + transform.position; // 親オブジェクトの位置を加算
                colliderObj.transform.parent = this.transform; // 親オブジェクトに設定

                BoxCollider boxCollider = colliderObj.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize); // サイズを調整
                segmentColliders.Add(colliderObj);
            }
        }


        void OnDrawGizmos()
        {
            if (!showGizmos) return; // showGizmosがfalseなら何もしない

            Gizmos.color = Color.red; // 色を設定

            Quaternion rotationQuaternion = Quaternion.Euler(rotation); // 回転をクォータニオンに変換

            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius + height, 0);

                // 回転を適用
                position = rotationQuaternion * position;

                // 中心位置のオフセットを加算
                position += centerOffset;

                Gizmos.DrawSphere(position + transform.position, debugSize); // 各セグメントに球を描画
            }

            // アーチを線でつなぐ
            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                float angle2 = Mathf.Lerp(0, Mathf.PI, (float)(i + 1) / segments);
                Vector3 pos1 = new Vector3(Mathf.Sin(angle1) * radius, Mathf.Cos(angle1) * radius + height, 0);
                Vector3 pos2 = new Vector3(Mathf.Sin(angle2) * radius, Mathf.Cos(angle2) * radius + height, 0);

                // 回転を適用
                pos1 = rotationQuaternion * pos1;
                pos2 = rotationQuaternion * pos2;

                // 中心位置のオフセットを加算
                pos1 += centerOffset;
                pos2 += centerOffset;

                Gizmos.DrawLine(pos1 + transform.position, pos2 + transform.position); // セグメント間を線でつなぐ
            }
        }

        void Update()
        {
            // アーチの位置を更新する場合は、ここにロジックを追加
            CreateColliders(); // 必要に応じてコライダーを再生成
        }
    }
}
