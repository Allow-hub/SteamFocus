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
        private Camera playerCamera;  // カメラへの参照を追加

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;
        [SerializeField] private float upwardForce = 5f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3;

        private void Awake()
        {
            Physics.gravity = localGravity;

            playerInput = GetComponent<PlayerInputController>();
            rb = GetComponent<Rigidbody>();
            playerCamera = Camera.main;
            Debug.Log(playerCamera);
        }

        private void Update()
        {
            // 入力を直接PlayerController内で処理
            HandleMovement();
            HandleJump();
            HandleAttack();
        }

        private void HandleMovement()
        {
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed * Time.deltaTime;

            if (movement == Vector3.zero) return;

            // カメラの水平方向の向きを基準に移動ベクトルを変換
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed * Time.deltaTime;

            // プレイヤーの向きをカメラの方向に合わせる
            Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Rigidbodyを使って移動
            rb.MovePosition(rb.position + adjustedMovement);
        }

        private void HandleJump()
        {
            if (playerInput.IsJumping)
            {
                Jump();
            }
        }

        private void HandleAttack()
        {
            if (playerInput.IsAttacking)
            {
                Attack();
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
    }
}
