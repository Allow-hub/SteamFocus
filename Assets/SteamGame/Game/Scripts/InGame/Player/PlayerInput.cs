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

        private float yMovement = 0f;  // Y軸移動の初期値



        private void Awake()
        {
            // ActionMapからアクションを取得
            var playerActionMap = inputActionAsset.FindActionMap("Player");
            moveAction = playerActionMap.FindAction("Move");
            attackAction = playerActionMap.FindAction("Attack");  // スペースキー用アクションを追加
            menuAction = playerActionMap.FindAction("Menu");

            // 入力アクションのコールバック設定
            moveAction.performed += OnMove;
            moveAction.canceled += ctx => moveInput = Vector3.zero;

            attackAction.performed += OnAttack;
        

            menuAction.performed += OnMenu;

        }

        private void OnEnable()
        {
            // アクションを有効化
            moveAction.Enable();
            attackAction.Enable();
            menuAction.Enable();
        }

        private void OnDisable()
        {
            // アクションを無効化
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
            // XZ平面とY軸の入力ベクトルを更新
            inputVector = new Vector3(moveInput.x, yMovement, moveInput.y);  // Y軸を調整
            isMoving = moveInput != Vector3.zero || yMovement != 0f;
        }

        // Moveアクションが実行されたときの処理 (XZ軸)
        private void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

        private void OnAttack(InputAction.CallbackContext context) => onAttackEvent?.Invoke();

        private void OnMenu(InputAction.CallbackContext context)
        {

        }
    }
}
