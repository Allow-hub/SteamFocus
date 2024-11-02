using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 2.0f;
        [SerializeField] private Transform player; // プレイヤーのTransform
        [SerializeField] private float distance = 5.0f; // プレイヤーからの距離
        [SerializeField] private float height = 2.0f; // カメラの高さ
        private Camera cam;
        private float rotationX = 0.0f; // 上下回転を保持する変数
        private float rotationY = 0.0f; // 左右回転を保持する変数
        private const float minYAngle = -90.0f; // 最小角度
        private const float maxYAngle = 90.0f;  // 最大角度

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            // マウスの動きを取得
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // 上下回転を更新
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle); // 90度の制限を適用

            // 左右回転を更新
            rotationY += mouseX;

            // プレイヤーの位置を基にカメラの位置を計算
            Vector3 offset = new Vector3(0, height, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            cam.transform.position = player.position + rotation * offset;

            // カメラの回転を更新
            cam.transform.LookAt(player.position + Vector3.up * height); // プレイヤーの位置を見つめる
        }
    }
}
