using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class AutoFollowCameraArm : MonoBehaviour
    {
        [SerializeField] private GameObject player; // プレイヤーオブジェクト
        [SerializeField] private float distance = 5f; // プレイヤーとの距離
        [SerializeField] private float height = 2f; // カメラの高さ
        [SerializeField] private float smoothSpeed = 0.1f; // カメラのスムーズさ
        [SerializeField] private float rotationSpeed = 100f; // 回転速度
        [SerializeField] private float verticalAngleLimit = 80f; // 垂直回転の制限

        private float currentRotationY; // 現在のY軸回転
        private float currentRotationX; // 現在のX軸回転

        private void Start()
        {
            currentRotationY = player.transform.eulerAngles.y; // 初期Y軸回転を設定
        }

        private void Update()
        {
            // マウスの移動量を取得
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Y軸回転を更新
            currentRotationY += mouseX;

            // X軸回転を更新（上下方向）
            currentRotationX -= mouseY;
            currentRotationX = Mathf.Clamp(currentRotationX, -verticalAngleLimit, verticalAngleLimit); // 制限を適用
        }

        private void LateUpdate()
        {
            // プレイヤーの位置を基準にカメラの位置を計算
            Vector3 playerPosition = player.transform.position + Vector3.up * height;
            Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            Vector3 targetPosition = playerPosition - rotation * Vector3.forward * distance;

            // カメラの位置をスムーズに更新
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.LookAt(playerPosition); // 常にプレイヤーを向く
        }
    }
}
