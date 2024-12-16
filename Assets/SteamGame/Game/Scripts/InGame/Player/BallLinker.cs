using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    ///     �{�[���Ƃ̐ڑ�����
    ///     �����߂�I�u�W�F�N�g�ɓK�p���邱��
    /// </summary>
    public class BallLinker : MonoBehaviour
    {
        /// <summary>
        ///     �ڑ���{�[��
        /// </summary>
        [SerializeField]
        private Ball ball;

        /// <summary>
        ///     �����W��
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

            // �J�v�Z���`��̗��[�Ń{�[���Ƃ̋����`�˗͂��v�Z
            var radius = ball.Radius - _collider.radius;
            var depth = Vector3.zero;
            var num = 0;

            // ���̂߂荞�ݗʂ��v�Z
            var capsuleTopDistance = Vector3.Distance(ballCenter, capsuleTop);
            if (capsuleTopDistance > radius)
            {
                depth += CalculatePenetration(capsuleTop, capsuleTopDistance - radius);
                num++;
            }

            // �����̂߂荞�ݗʂ��v�Z
            var capsuleBottomDistance = Vector3.Distance(ballCenter, capsuleBottom);
            if (capsuleBottomDistance > radius)
            {
                depth += CalculatePenetration(capsuleBottom, capsuleBottomDistance - radius);
                num++;
            }

            // �߂荞�݂�����Ώ���
            if (num > 0)
            {
                // ����ꂽ�߂荞�ݗʂ̕��ω�
                depth /= num;

                // ���g�̂߂荞�݂�����
                _rb.MovePosition(rbPosition + depth);

                // ���z�̖@�������ƂɃ{�[���Ǝ����ƂŃG�l���M�[������
                var normal = depth.normalized;
                var relativeVelocity = _rb.velocity - ball.Rb.velocity;
                var velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);

                // �{�[���ƃv���C���[�Ԃő��x���t�����̏ꍇ�̂ݏ���
                if (velocityAlongNormal > 0)
                    return;

                // �Փˎ��̃G�l���M�[����
                var j = -(1 + restitution) * velocityAlongNormal;
                j /= 1 / _rb.mass + 1 / ball.Rb.mass; // ���ʂɂ�钲��

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
