using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class TowerDebris : MonoBehaviour
    {
        [SerializeField] private GameObject[] debrisPrefabs; // デブリのPrefab配列
        [SerializeField] private int debrisCount = 10;       // デブリの数
        [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f); // 球体のスケール
        [SerializeField] private float exclusionRadius = 5f; // 中心から除外する半径
        [SerializeField] private Vector3 debrisMinSize = new Vector3(0.5f, 0.5f, 0.5f); // デブリの最小サイズ
        [SerializeField] private Vector3 debrisMaxSize = new Vector3(2f, 2f, 2f);       // デブリの最大サイズ
        [SerializeField] private Transform parentObject;     // 親オブジェクト
        [SerializeField] private bool drawGizmo = true;      // Gizmoを描画するかどうか
        [SerializeField] private Color gizmoColor = Color.cyan; // Gizmoの色

        /// <summary>
        /// デブリを下半分の球体内（指定した半径を除外）にランダムに配置します。
        /// </summary>
        public void GenerateDebris()
        {
            if (debrisPrefabs == null || debrisPrefabs.Length == 0)
            {
                Debug.LogError("Debris Prefabs are not assigned!");
                return;
            }

            // 親オブジェクトの子オブジェクトを削除（親オブジェクトが指定されている場合）
            if (parentObject != null)
            {
                foreach (Transform child in parentObject)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            int createdCount = 0;
            while (createdCount < debrisCount)
            {
                float angleHorizontal = Random.Range(0, 2 * Mathf.PI); // 横方向の角度（0〜2π）
                float angleVertical = Random.Range(Mathf.PI / 2, Mathf.PI); // 縦方向の角度（π/2〜π）
                float randomRadius = Random.value; // 半径をランダム化（0〜1）

                // 球体座標から位置を計算
                float x = randomRadius * Mathf.Sin(angleVertical) * Mathf.Cos(angleHorizontal) * spawnAreaSize.x;
                float y = randomRadius * Mathf.Cos(angleVertical) * spawnAreaSize.y;
                float z = randomRadius * Mathf.Sin(angleVertical) * Mathf.Sin(angleHorizontal) * spawnAreaSize.z;
                Vector3 position = transform.position + new Vector3(x, y, z); // 球体の中心を transform.position に設定

                // 中心からの距離を計算し、除外半径を満たしているか確認
                if (Vector3.Distance(transform.position, position) < exclusionRadius)
                {
                    continue; // 除外半径内の場合、スキップ
                }

                // ランダムなPrefabを選択して配置
                GameObject selectedPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];
                GameObject debris = Instantiate(selectedPrefab, position, Quaternion.identity);
                debris.name = $"Debris_{createdCount}";

                // デブリのランダムなサイズを設定
                float randomX = Random.Range(debrisMinSize.x, debrisMaxSize.x);
                float randomY = Random.Range(debrisMinSize.y, debrisMaxSize.y);
                float randomZ = Random.Range(debrisMinSize.z, debrisMaxSize.z);
                debris.transform.localScale = new Vector3(randomX, randomY, randomZ);

                if (parentObject != null)
                {
                    debris.transform.SetParent(parentObject);
                }

                createdCount++;
            }

            Debug.Log("Debris generated successfully!");
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo) return;

            Gizmos.color = gizmoColor;

            // 球体のGizmoを描画
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, spawnAreaSize);
            Gizmos.DrawWireSphere(Vector3.zero, 1);

            // 除外領域のGizmoを描画
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, exclusionRadius);

            // 下半分の球体を視覚化
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.5f); // 半透明
            for (float theta = Mathf.PI / 2; theta <= Mathf.PI; theta += Mathf.PI / 10)
            {
                for (float phi = 0; phi <= 2 * Mathf.PI; phi += Mathf.PI / 10)
                {
                    Vector3 point = new Vector3(
                        Mathf.Sin(theta) * Mathf.Cos(phi),
                        Mathf.Cos(theta),
                        Mathf.Sin(theta) * Mathf.Sin(phi)
                    );
                    Gizmos.DrawSphere(transform.position + Vector3.Scale(point, spawnAreaSize), 0.05f);
                }
            }
        }
    }
}
