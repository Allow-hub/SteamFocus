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
        [SerializeField] private float moveSpeed = 5f;            // ��{�ړ����x
        [SerializeField] private float rotationSpeed = 2f;        // ��]���x
        [SerializeField] private float maxSpeed = 10f;            // �ő呬�x
        [SerializeField] private float accelerationFactor = 5f;   // �����W��
        [SerializeField] private float decelerationFactor = 5f;  // �����W��
        [SerializeField] private float limitSpeed = 10f;         // �ړ��̐������x
        [SerializeField] private float brakeForce = 15f;         // �t�����̗́i�u���[�L�j�̋���

        private Camera playerCamera;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;        // �U�����̑O����
        [SerializeField] private float upwardForce = 5f;          // �U�����̏����
        [SerializeField] private float attackCoolTime = 1f;       // �U���̃N�[���^�C��
        [SerializeField] private float attackTime = 1f;           // �A�^�b�N��ɑҋ@���鎞��
        private bool canAttack = true;
        private bool isTaking = false;                             // �A�^�b�N���t���O

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3f;            // �W�����v��
        [SerializeField] private float jumpCoolTime = 1f;         // �W�����v�̃N�[���^�C��
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
            // �W�����v��^�b�N���̏���
            HandleJump();
            HandleAttack();
        }

        private void FixedUpdate()
        {
            // �������Z���g�����ړ�����
            HandleMovement();
        }

        private void HandleMovement()
        {
            // ���͂Ɋ�Â��ړ��x�N�g�����v�Z
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed;

            if (movement == Vector3.zero) return;

            // �J�����̌����Ɋ�Â��Ĉړ��x�N�g���𒲐�
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed;

            // �����x�Ɋ�Â��Ĉړ��x�N�g���𒲐�
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = adjustedMovement * moveSpeed;

            // ���݂̑��x�ƖڕW���x�̍���������E�����𐧌�
            Vector3 velocityDifference = targetVelocity - currentVelocity;
            Vector3 acceleration = velocityDifference.normalized * accelerationFactor * Time.deltaTime;

            if (velocityDifference.magnitude < 0)
            {
                // ����
                acceleration = velocityDifference.normalized * decelerationFactor * Time.deltaTime;
            }

            // �ړ������ɗ͂�������i�����I�Ȉړ��j
            rb.AddForce(acceleration, ForceMode.VelocityChange);

            // �ő呬�x�𒴂����ꍇ�A�t�����Ƀu���[�L��������
            if (new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude > maxSpeed)
            {
                // ���݂̑��x�ɑ΂��ċt�����̗͂�������
                Vector3 brakeDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;
                Vector3 brakeForceDirection = -brakeDirection * brakeForce;

                // �t�����̗͂�������
                rb.AddForce(brakeForceDirection, ForceMode.Acceleration);
            }

            // ���x�̐���
            if (new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude > maxSpeed && !isTaking)
            {
                Vector3 clampedVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized * maxSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, currentVelocity.y, clampedVelocity.z);
            }

            // �v���C���[���ړ������Ɍ�����
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
                isTaking = true;  // �A�^�b�N���t���O�𗧂Ă�
                StartCoroutine(CoolTime(false));
                StartCoroutine(AttackDelay());  // �A�^�b�N��̒x�����J�n
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
            // �A�^�b�N��A�w�莞�ԑҋ@
            yield return new WaitForSeconds(attackTime);
            isTaking = false;  // �A�^�b�N�I��
        }
    }
}
