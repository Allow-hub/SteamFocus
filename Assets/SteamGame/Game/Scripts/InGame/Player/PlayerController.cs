using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputManager playerInput;
        private Rigidbody rb;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 2;
        [SerializeField] private Camera playerCamera;  // カメラへの参照を追加

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;
        [SerializeField] private float upwardForce = 5f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInputManager>();
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            PlayerInputManager.onAttackEvent += Attack;
            PlayerInputManager.onJumpEvent += Jump;
        }

        private void OnDisable()
        {
            PlayerInputManager.onAttackEvent -= Attack;
            PlayerInputManager.onJumpEvent -= Jump;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed * Time.fixedDeltaTime;

            if (movement == Vector3.zero) return;

            // カメラの水平方向の向きを基準に移動ベクトルを変換
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed * Time.fixedDeltaTime;

            // プレイヤーの向きをカメラの方向に合わせる
            Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Rigidbodyを使って移動
            rb.MovePosition(rb.position + adjustedMovement);
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
