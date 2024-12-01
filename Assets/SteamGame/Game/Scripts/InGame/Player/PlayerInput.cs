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
        public bool IsAttacking => isAttacking;  // �U����Ԃ��Ǘ�����

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
        //    // ActionMap����A�N�V�������擾
        //    var playerActionMap = inputActionAsset.FindActionMap("Player");
        //    moveAction = playerActionMap.FindAction("Move");
        //    jumpAction = playerActionMap.FindAction("Jump");
        //    attackAction = playerActionMap.FindAction("Attack");
        //    menuAction = playerActionMap.FindAction("Menu");

        //    // ���̓A�N�V�����̃R�[���o�b�N�ݒ�
        //    moveAction.performed += OnMove;
        //    moveAction.canceled += ctx => moveInput = Vector3.zero;

        //    jumpAction.performed += OnJump;
        //    attackAction.performed += OnAttack;
        //    menuAction.performed += OnMenu;
        //}

        //private void OnEnable()
        //{
        //    // �A�N�V������L����
        //    moveAction.Enable();
        //    jumpAction.Enable();
        //    attackAction.Enable();
        //    menuAction.Enable();
        //}

        //private void OnDisable()
        //{
        //    // �A�N�V�����𖳌���
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
            // XZ���ʂ�Y���̓��̓x�N�g�����X�V
            inputVector = new Vector3(moveInput.x, yMovement, moveInput.y);
            isMoving = moveInput != Vector3.zero || yMovement != 0f;
        }

        // Move�A�N�V���������s���ꂽ�Ƃ��̏��� (XZ��)
        public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

        // Jump�A�N�V���������s���ꂽ�Ƃ��̏��� (Y����+1�ɂ���)
        public void OnJump(InputAction.CallbackContext context)
        {
            isJumping = true;  // �W�����v��Ԃ�true��
            StartCoroutine(Delay(0));
        }

        // Attack�A�N�V���������s���ꂽ�Ƃ��̏���
        public void OnAttack(InputAction.CallbackContext context)
        {
            isAttacking = true;  // �U����Ԃ�true��
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
        // Menu�A�N�V�����i���j���[�֘A�̏����j
        public void OnMenu(InputAction.CallbackContext context)
        {
            if (GameManager.I == null) return;
            if (GameManager.I.currentState == GameManager.GameState.Menu)
                GameManager.I.ChangeLastState();
            else
                GameManager.I.ChangeMenuState();
        }

        // �����ŏ�Ԃ����Z�b�g���邽�߂̃��\�b�h��ǉ��ł��܂�
        public void ResetJumping() => isJumping = false;
        public void ResetAttacking() => isAttacking = false;
    }
}
