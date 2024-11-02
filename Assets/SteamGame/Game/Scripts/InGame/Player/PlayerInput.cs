using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TechC
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private InputActionAsset inputActionAsset;


        public delegate void OnAttackEvent();
        public static OnAttackEvent onAttackEvent;

        public Vector3 InputVector => inputVector;
        public bool IsMoving => isMoving;

        private Vector3 inputVector;
        private Vector3 moveInput;
        private bool isMoving = false;
        private InputAction moveAction;
        private InputAction attackAction;
        private InputAction menuAction;

        private float yMovement = 0f;  // Y���ړ��̏����l



        private void Awake()
        {
            // ActionMap����A�N�V�������擾
            var playerActionMap = inputActionAsset.FindActionMap("Player");
            moveAction = playerActionMap.FindAction("Move");
            attackAction = playerActionMap.FindAction("Attack");  // �X�y�[�X�L�[�p�A�N�V������ǉ�
            menuAction = playerActionMap.FindAction("Menu");

            // ���̓A�N�V�����̃R�[���o�b�N�ݒ�
            moveAction.performed += OnMove;
            moveAction.canceled += ctx => moveInput = Vector3.zero;

            attackAction.performed += OnAttack;
        

            menuAction.performed += OnMenu;

        }

        private void OnEnable()
        {
            // �A�N�V������L����
            moveAction.Enable();
            attackAction.Enable();
            menuAction.Enable();
        }

        private void OnDisable()
        {
            // �A�N�V�����𖳌���
            moveAction.Disable();
            attackAction.Disable();
            menuAction.Disable();
        }


        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // XZ���ʂ�Y���̓��̓x�N�g�����X�V
            inputVector = new Vector3(moveInput.x, yMovement, moveInput.y);  // Y���𒲐�
            isMoving = moveInput != Vector3.zero || yMovement != 0f;
        }

        // Move�A�N�V���������s���ꂽ�Ƃ��̏��� (XZ��)
        private void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

        private void OnAttack(InputAction.CallbackContext context) => onAttackEvent?.Invoke();

        private void OnMenu(InputAction.CallbackContext context)
        {

        }
    }
}
