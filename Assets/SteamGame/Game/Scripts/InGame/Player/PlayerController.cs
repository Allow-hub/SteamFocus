using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput playerInput;
        private Rigidbody rb;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 2;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;  // �^�b�N���̑O���̗�
        [SerializeField] private float upwardForce = 5f;    // �^�b�N���̏���̗�


        
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            rb = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            PlayerInput.onAttackEvent += Attack;
        }

        private void OnDisable()
        {
            PlayerInput.onAttackEvent -= Attack;         
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
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + movement);
        }

        /// <summary>
        /// �^�b�N��
        /// </summary>
        private void Attack()
        {
            Debug.Log("A");
            // �^�b�N���̗͂��v�Z
            Vector3 tackleDirection = transform.forward * forwardForce + Vector3.up * upwardForce;
            rb.AddForce(tackleDirection, ForceMode.Impulse); // �C���p���X�ŗ͂�K�p
        }
    }
}
