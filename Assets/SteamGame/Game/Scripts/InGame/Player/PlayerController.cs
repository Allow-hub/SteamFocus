using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector3 localGravity;

        private PlayerInputController playerInput;
        private Rigidbody rb;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;            // 基本移動速度
        [SerializeField] private float rotationSpeed = 2f;        // 回転速度
        [SerializeField] private float maxSpeed = 10f;            // 最大速度
        [SerializeField] private float accelerationFactor = 5f;   // 加速係数
        [SerializeField] private float decelerationFactor = 5f;  // 減速係数
        [SerializeField] private float limitSpeed = 10f;         // 移動の制限速度
        [SerializeField] private float brakeForce = 15f;         // 逆方向の力（ブレーキ）の強さ

        private Camera playerCamera;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;        // 攻撃時の前方力
        [SerializeField] private float upwardForce = 5f;          // 攻撃時の上方力
        [SerializeField] private float attackCoolTime = 1f;       // 攻撃のクールタイム
        [SerializeField] private float attackTime = 1f;           // アタック後に待機する時間
        private bool canAttack = true;
        private bool isTaking = false;                             // アタック中フラグ

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3f;            // ジャンプ力
        [SerializeField] private float jumpCoolTime = 1f;         // ジャンプのクールタイム
        private bool canJump = true;

        private void Awake()
        {
            Physics.gravity = localGravity;

            playerInput = GetComponent<PlayerInputController>();
            rb = GetComponent<Rigidbody>();
            playerCamera = Camera.main;
        }

        private void Update()
        {
            // ジャンプやタックルの処理
            HandleJump();
            HandleAttack();
        }

        private void FixedUpdate()
        {
            // 物理演算を使った移動処理
            HandleMovement();
        }

        private void HandleMovement()
        {
            // 入力に基づく移動ベクトルを計算
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed;

            if (movement == Vector3.zero) return;

            // カメラの向きに基づいて移動ベクトルを調整
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed;

            // 加速度に基づいて移動ベクトルを調整
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = adjustedMovement * moveSpeed;

            // 現在の速度と目標速度の差から加速・減速を制御
            Vector3 velocityDifference = targetVelocity - currentVelocity;
            Vector3 acceleration = velocityDifference.normalized * accelerationFactor * Time.deltaTime;

            if (velocityDifference.magnitude < 0)
            {
                // 減速
                acceleration = velocityDifference.normalized * decelerationFactor * Time.deltaTime;
            }

            // 移動方向に力を加える（物理的な移動）
            rb.AddForce(acceleration, ForceMode.VelocityChange);

            // 最大速度を超えた場合、逆方向にブレーキをかける
            if (new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude > maxSpeed)
            {
                // 現在の速度に対して逆方向の力を加える
                Vector3 brakeDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;
                Vector3 brakeForceDirection = -brakeDirection * brakeForce;

                // 逆方向の力を加える
                rb.AddForce(brakeForceDirection, ForceMode.Acceleration);
            }

            // 速度の制限
            if (new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude > maxSpeed && !isTaking)
            {
                Vector3 clampedVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized * maxSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, currentVelocity.y, clampedVelocity.z);
            }

            // プレイヤーを移動方向に向ける
            if (adjustedMovement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleJump()
        {
            if (playerInput.IsJumping && canJump)
            {
                Jump();
                canJump = false;
                StartCoroutine(CoolTime(true));
            }
        }

        private void HandleAttack()
        {
            if (playerInput.IsAttacking && canAttack && !isTaking)
            {
                Attack();
                canAttack = false;
                isTaking = true;  // アタック中フラグを立てる
                StartCoroutine(CoolTime(false));
                StartCoroutine(AttackDelay());  // アタック後の遅延を開始
            }
        }

        private void Attack()
        {
            Vector3 tackleDirection = transform.forward * forwardForce + Vector3.up * upwardForce;
            rb.AddForce(tackleDirection, ForceMode.Impulse);
        }

        private void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private IEnumerator CoolTime(bool isJump)
        {
            if (isJump)
            {
                yield return new WaitForSeconds(jumpCoolTime);
                canJump = true;
            }
            else
            {
                yield return new WaitForSeconds(attackCoolTime);
                canAttack = true;
            }
        }

        private IEnumerator AttackDelay()
        {
            // アタック後、指定時間待機
            yield return new WaitForSeconds(attackTime);
            isTaking = false;  // アタック終了
        }
    }
}
