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
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 2;
        [SerializeField] private float maxSpeed = 10f;  // 最大速度を追加
        [SerializeField] private float limitSpeed = 10f; 

        private Camera playerCamera;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;
        [SerializeField] private float upwardForce = 5f;
        [SerializeField] private float attackCoolTime = 1f;
        private bool canAttack = true;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3;
        [SerializeField] private float jumpCoolTime = 1f;
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
            //if (!canJump && canAttack) return;
            // 入力に基づく移動ベクトルを計算
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed;

            if (movement == Vector3.zero) return;

            // カメラの向きに基づいて移動ベクトルを調整
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed;

            // 移動方向に力を加える（物理的な移動）
            rb.AddForce(adjustedMovement, ForceMode.VelocityChange);

            Vector3 currentVelocity = rb.velocity;
            if (new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude > maxSpeed)
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
            if (playerInput.IsAttacking && canAttack)
            {
                Attack();
                canAttack = false;
                StartCoroutine(CoolTime(false));
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
    }
}
