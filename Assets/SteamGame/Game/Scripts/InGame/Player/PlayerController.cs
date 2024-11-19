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
        [SerializeField] private SphereCollider sphereCollider; //ボールのクランプ位置
        private Vector3 center; // SphereColliderの中心
        private float radius; // SphereColliderの半径
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
            // SphereColliderの中心と半径を取得
            center = sphereCollider.transform.position + sphereCollider.center;
            radius = sphereCollider.radius;
        }

        private void Update()
        {
            // 入力を直接PlayerController内で処理
            HandleMovement();
            HandleJump();
            HandleAttack();
            //ClampPlayerPosition();
        }

        private void ClampPlayerPosition()
        {
            // プレイヤーがSphereColliderの外に出ているかチェック
            if (Vector3.Distance(transform.position, center) > radius)
            {
                // プレイヤーをSphereColliderの内側にクランプ
                Vector3 direction = (transform.position - center).normalized; // 中心からプレイヤーへの方向
                transform.position = center + direction * radius; // 新しい位置を計算
            }
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
            if (playerInput.IsJumping&&canJump)
            {
                Jump();
                canJump = false; 
                StartCoroutine(CoolTime(true));
            }
        }

        private void HandleAttack()
        {
            if (playerInput.IsAttacking&&canAttack)
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
