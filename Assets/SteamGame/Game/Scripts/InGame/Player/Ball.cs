using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float checkRadius = 10f;  // チェックする範囲の半径
        [SerializeField] private Vector3 checkCenter;      // チェックの中心座標（通常はボールの位置）

        private SphereCollider sphereCollider;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                Debug.LogError("SphereColliderがアタッチされていません");
            }
        }

        private void FixedUpdate()
        {
            // ボールが指定の範囲を外れたかどうかをチェック
            CheckIfOutsideCollider();
        }

        private void CheckIfOutsideCollider()
        {
            // SphereColliderの中心から指定した範囲（checkRadius）を外れているかどうか
            bool isOutside = !Physics.CheckSphere(checkCenter, checkRadius, LayerMask.GetMask("Default")); // "Default" は変更可能

            // 外れている場合の処理
            if (isOutside)
            {
                OnExitCollider();
            }
        }

        private void OnExitCollider()
        {
            // ボールが指定範囲外に出た場合の処理
            Debug.Log("ボールが指定範囲外に出ました");
            // ここで適切な処理を追加
        }

        // Gizmoで表示
        private void OnDrawGizmos()
        {
            // Gizmosの色を設定
            Gizmos.color = Color.red;

            // ボールの位置を中心にして、checkRadiusの範囲を可視化
            Gizmos.DrawWireSphere(checkCenter, checkRadius);
        }
    }
}
