using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    ///     ボールとの接続処理
    ///     閉じ込めるオブジェクトに適用すること
    /// </summary>
    public class BallLinker : MonoBehaviour
    {
        /// <summary>
        ///     接続先ボール
        /// </summary>
        [SerializeField]
        private Ball ball;

        /// <summary>
        ///     反発係数
        /// </summary>
        [Range(0f, 1f)]
        [SerializeField]
        private float restitution = 0.5f;

        private Rigidbody _rb;
        private CapsuleCollider _collider;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();

            if (ball == null)
                ball = FindFirstObjectByType<Ball>();
        }

        private void Update()
        {
            var rbPosition = _rb.position;

            var capsuleAxis = _rb.rotation * _collider.direction switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.zero
            };

            capsuleAxis *= _collider.height * 0.5f - _collider.radius;

            var colliderCenter = rbPosition + _rb.rotation * _collider.center;
            var capsuleTop = colliderCenter + capsuleAxis;
            var capsuleBottom = colliderCenter - capsuleAxis;

            var ballCenter = ball.Rb.position;
            Debug.DrawLine(ballCenter, capsuleTop, Color.yellow);
            Debug.DrawLine(ballCenter, capsuleBottom, Color.yellow);

            // カプセル形状の両端でボールとの距離～斥力を計算
            var radius = ball.Radius - _collider.radius;
            var depth = Vector3.zero;
            var num = 0;

            // 頭のめり込み量を計算
            var capsuleTopDistance = Vector3.Distance(ballCenter, capsuleTop);
            if (capsuleTopDistance > radius)
            {
                depth += CalculatePenetration(capsuleTop, capsuleTopDistance - radius);
                num++;
            }

            // 足元のめり込み量を計算
            var capsuleBottomDistance = Vector3.Distance(ballCenter, capsuleBottom);
            if (capsuleBottomDistance > radius)
            {
                depth += CalculatePenetration(capsuleBottom, capsuleBottomDistance - radius);
                num++;
            }

            // めり込みがあれば処理
            if (num > 0)
            {
                // 得られためり込み量の平均化
                depth /= num;

                // 自身のめり込みを解消
                _rb.MovePosition(rbPosition + depth);

                // 仮想の法線をもとにボールと自分とでエネルギーを交換
                var normal = depth.normalized;
                var relativeVelocity = _rb.velocity - ball.Rb.velocity;
                var velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);

                // ボールとプレイヤー間で速度が逆方向の場合のみ処理
                if (velocityAlongNormal > 0)
                    return;

                // 衝突時のエネルギー交換
                var j = -(1 + restitution) * velocityAlongNormal;
                j /= 1 / _rb.mass + 1 / ball.Rb.mass; // 質量による調整

                var impulse = normal * j;

                _rb.AddForce(impulse, ForceMode.Impulse);
                ball.Rb.AddForce(-impulse, ForceMode.Impulse);
            }

            Vector3 CalculatePenetration(Vector3 point, float penetration)
            {
                var dir = (ballCenter - point).normalized;
                return dir * penetration;
            }
        }
    }
}
