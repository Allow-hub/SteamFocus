using System.Collections;
using UnityEngine;

namespace TechC
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 2.0f;
        [SerializeField] private Transform player;
        [SerializeField] private float distance = 5.0f;
        [SerializeField] private float height = 2.0f;
        [SerializeField] private float shakeDuration = 0.5f; // シェイクの時間
        [SerializeField] private float shakeMagnitude = 0.3f; // シェイクの強度
        [SerializeField] private float dampingSpeed = 1.0f; // 減衰スピード

        private Camera cam;
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private const float minYAngle = -90.0f;
        private const float maxYAngle = 90.0f;
        private Vector3 shakeOffset = Vector3.zero;
        private float initialShakeMagnitude;

        private void Awake()
        {
            cam = Camera.main;
            initialShakeMagnitude = shakeMagnitude; // 初期のシェイク強度を保存
        }

        private void Update()
        {
            // マウスの動きを取得
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // 上下回転を更新
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);

            // 左右回転を更新
            rotationY += mouseX;

            // プレイヤーの位置を基にカメラの位置を計算
            Vector3 offset = new Vector3(0, height, -distance);
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            cam.transform.position = player.position + rotation * offset + shakeOffset;

            // カメラの回転を更新
            cam.transform.LookAt(player.position + Vector3.up * height);
        }

        public void TriggerShake()
        {
            StopAllCoroutines(); // 既存のシェイクがあれば停止
            StartCoroutine(Shake());
        }

        private IEnumerator Shake()
        {
            float elapsed = 0.0f;
            shakeMagnitude = initialShakeMagnitude; // シェイク強度を初期化

            while (elapsed < shakeDuration)
            {
                // 徐々にシェイクの強度を減衰させる
                float damper = 1.0f - (elapsed / shakeDuration);

                // Perlinノイズを使ったスムーズな揺れ
                float shakeX = (Mathf.PerlinNoise(Time.time * dampingSpeed, 0) - 0.5f) * 2 * shakeMagnitude * damper;
                float shakeY = (Mathf.PerlinNoise(0, Time.time * dampingSpeed) - 0.5f) * 2 * shakeMagnitude * damper;
                shakeOffset = new Vector3(shakeX, shakeY, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            shakeOffset = Vector3.zero; // シェイク終了時にオフセットをリセット
        }
    }
}
