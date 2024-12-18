using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    ///メモ Transform型は参照型なので初期化に使えない
    /// </summary>
    public class RepairOutWall : MonoBehaviour
    {
        private Vector3 initialPosition;   // 初期位置を保存
        private Quaternion initialRotation; // 初期回転を保存
        private Rigidbody rb;
        private void Awake()
        {
            // 初期状態を保存
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            rb=GetComponent<Rigidbody>();   
        }

        private void OnEnable()
        {
            // 初期状態にリセット
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
        }
    }
}
