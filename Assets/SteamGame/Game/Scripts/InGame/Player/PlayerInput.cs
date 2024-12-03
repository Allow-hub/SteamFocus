using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TechC
{
    public class PlayerInputController : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private InputActionAsset inputActionAsset;

        public Vector3 InputVector => inputVector;
        public bool IsMoving => isMoving;
        public bool IsJumping => isJumping;
        public bool IsAttacking => isAttacking;  // 攻撃状態を管理する

        private Vector3 inputVector;
        private Vector3 moveInput;
        private bool isMoving = false;
        private bool isJumping = false;
        private bool isAttacking = false;

        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction attackAction;
        private InputAction menuAction;

        private float yMovement = 0f;

        //private void Awake()
        //{
        //    // ActionMapからアクションを取得
        //    var playerActionMap = inputActionAsset.FindActionMap("Player");
        //    moveAction = playerActionMap.FindAction("Move");
        //    jumpAction = playerActionMap.FindAction("Jump");
        //    attackAction = playerActionMap.FindAction("Attack");
        //    menuAction = playerActionMap.FindAction("Menu");

        //    // 入力アクションのコールバック設定
        //    moveAction.performed += OnMove;
        //    moveAction.canceled += ctx => moveInput = Vector3.zero;

        //    jumpAction.performed += OnJump;
        //    attackAction.performed += OnAttack;
        //    menuAction.performed += OnMenu;
        //}

        //private void OnEnable()
        //{
        //    // アクションを有効化
        //    moveAction.Enable();
        //    jumpAction.Enable();
        //    attackAction.Enable();
        //    menuAction.Enable();
        //}

        //private void OnDisable()
        //{
        //    // アクションを無効化
        //    moveAction.Disable();
        //    jumpAction.Disable();
        //    attackAction.Disable();
        //    menuAction.Disable();
        //}

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // XZ平面とY軸の入力ベクトルを更新
            inputVector = new Vector3(moveInput.x, yMovement, moveInput.y);
            isMoving = moveInput != Vector3.zero || yMovement != 0f;
        }

        // Moveアクションが実行されたときの処理 (XZ軸)
        public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

        // Jumpアクションが実行されたときの処理 (Y軸を+1にする)
        public void OnJump(InputAction.CallbackContext context)
        {
            isJumping = true;  // ジャンプ状態をtrueに
            StartCoroutine(Delay(0));
        }

        // Attackアクションが実行されたときの処理
        public void OnAttack(InputAction.CallbackContext context)
        {
            isAttacking = true;  // 攻撃状態をtrueに
            StartCoroutine(Delay(1));
        }

        private IEnumerator Delay(int n)
        {
            yield return new WaitForSeconds(0.1f);
            if(n==0)
            {
                ResetJumping();
            }
            if (n == 1)
            {
                ResetAttacking();
            }
        }

        public void OnTeam()
        {
            Debug.Log("Team");
        }

        public void ReStart()
        {
            GameManager.I.LoadSceneAsync(1);
            GameManager.I.ChangeTitleState();
        }
        // Menuアクション（メニュー関連の処理）
        public void OnMenu(InputAction.CallbackContext context)
        {
            if (GameManager.I == null) return;
            if (GameManager.I.currentState == GameManager.GameState.Menu)
                GameManager.I.ChangeLastState();
            else
                GameManager.I.ChangeMenuState();
        }

        // ここで状態をリセットするためのメソッドを追加できます
        public void ResetJumping() => isJumping = false;
        public void ResetAttacking() => isAttacking = false;
    }
}
