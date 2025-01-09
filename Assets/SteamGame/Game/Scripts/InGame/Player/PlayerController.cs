using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Vector3 localGravity;
        [SerializeField] private float delayStart = 0.5f;
        [SerializeField] private Animator anim;
        private PlayerInputController playerInput;
        private Rigidbody rb;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;            // ��{�ړ����x
        [SerializeField] private float rotationSpeed = 2f;        // ��]���x
        [SerializeField] private float limit = 1.2f;             // �{�[���̑��x�̉��{�܂ŋ��e���邩
        [SerializeField] private float decelerationFactor = 2f;   // �����̋���
        [SerializeField] private Rigidbody ballRb;
        private const string walkAnimName = "IsWalking";

        private Camera playerCamera;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;        // �U�����̑O����
        [SerializeField] private float upwardForce = 5f;          // �U�����̏����
        [SerializeField] private float attackCoolTime = 1f;       // �U���̃N�[���^�C��
        [SerializeField] private float attackTime = 1f;           // �A�^�b�N��ɑҋ@���鎞��
        [SerializeField] private float teamAttackForwardForce;
        [SerializeField] private float teamAttackUpwardForce;

        private const string attackAnimName = "IsAttacking";
        private bool canAttack = true;
        private bool isTaking = false;                             // �A�^�b�N���t���O

        private float initForwardForce;
        private float initUpwardForce;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3f;            // �W�����v��
        [SerializeField] private float jumpForwardForce = 100f; 
        [SerializeField] private float jumpCoolTime = 1f;         // �W�����v�̃N�[���^�C��
        [SerializeField] private float teamJumpForce;

        private const string jumpAnimName = "IsJumping";
        private bool canJump = true;

        private float initJumpForce;

        public override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(DelayStart());
        }

        private void Awake()
        {
            if(ballRb == null)
            {
              ballRb =  GameObject.Find("Ball").gameObject.GetComponent<Rigidbody>();
            }
            //Physics.gravity = localGravity;

            playerInput = GetComponent<PlayerInputController>();
            rb = GetComponent<Rigidbody>();
            playerCamera = Camera.main;
            initForwardForce = forwardForce;
            initUpwardForce = upwardForce;
            initJumpForce = jumpForce;

        }

        private void Update()
        {
            if (!GameManager.I.canPlay) return;
            if (photonView.IsMine)
            {
                // �W�����v��^�b�N���̏���
                HandleJump();
                HandleAttack();
                HandleMovement();

            }
        }

        private void FixedUpdate()
        {
            if (!GameManager.I.canPlay) return;

            if (photonView.IsMine)
            {
                // �������Z���g�����ړ�����
                //HandleMovement();
                //LimitSpeed();
            }
        }

        private void HandleMovement()
        {
            // ���͂Ɋ�Â��ړ��x�N�g�����v�Z
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed;

            if (inputVector == Vector3.zero)
            {
                // �~�܂��Ă���ꍇ�A�ړ��A�j���[�V�������~
                anim.SetBool(walkAnimName, false);
                return;
            }

            // �J�����̌����Ɋ�Â��Ĉړ��x�N�g���𒲐�
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed;

            // �v���C���[�̈ړ�
            // ���̕������Z�Ƃ̊���h�����߂ɍ��W����ňړ������Ă���
            rb.MovePosition(rb.position + adjustedMovement * Time.deltaTime);
            
            // �v���C���[���ړ������Ɍ�����
            if (adjustedMovement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // �ړ����̓A�j���[�V�����𔭉�
            anim.SetBool(walkAnimName, true);
        }

        private void LimitSpeed()
        {
            float ballSpeed = ballRb.velocity.magnitude;
            if (ballSpeed == 0) return;  // �{�[������~���Ă���ꍇ�A�v�Z���s��Ȃ�

            float speedRatio = rb.velocity.magnitude / (ballSpeed * limit);

            // speedRatio��NaN�܂��͖�����ɂȂ�Ȃ��悤�Ƀ`�F�b�N
            if (float.IsNaN(speedRatio) || float.IsInfinity(speedRatio) || speedRatio <= 1)
            {
                return;  // �����Ȓl�̏ꍇ�͏������X�L�b�v
            }
            // �v���C���[�̑��x���{�[���̑��x��n�{�𒴂��Ă���ꍇ
            Vector3 reverseForce = -rb.velocity.normalized * moveSpeed * (speedRatio - 1);

            // �����̋����𒲐�����
            rb.AddForce(reverseForce * decelerationFactor * Time.deltaTime, ForceMode.Acceleration);
        }

        private void HandleJump()
        {
            if (playerInput.IsJumping && canJump)
            {
                Jump();
                canJump = false;
                anim.SetTrigger(jumpAnimName);  
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
                anim.SetTrigger(attackAnimName);  
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
            //Break(Vector3.zero);
            // �W�����v�̗�
            Vector3 jumpForceVector = Vector3.up * jumpForce;

            // �O�����̗�
            Vector3 forwardForceVector = transform.forward * jumpForwardForce;
            // ���������͂�������
            rb.AddForce(jumpForceVector + forwardForceVector, ForceMode.Impulse);
        }

        private IEnumerator CoolTime(bool isJump)
        {
            if (isJump)
            {
                yield return new WaitForSeconds(jumpCoolTime);
                anim.SetBool(jumpAnimName,false);

                canJump = true;
            }
            else
            {
                yield return new WaitForSeconds(attackCoolTime);
                anim.SetBool(attackAnimName,false);

                canAttack = true;
            }
        }

        private IEnumerator AttackDelay()
        {
            // �A�^�b�N��A�w�莞�ԑҋ@
            yield return new WaitForSeconds(attackTime);
            isTaking = false;  // �A�^�b�N�I��
        }

        //public void Break(Vector3 checkPoint)
        //{
        //    // �v���C���[�I�u�W�F�N�g��S�擾
        //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        Rigidbody playerRb = players[i].GetComponent<Rigidbody>();

        //        if (playerRb != null)
        //        {
        //            Debug.Log(players.Length);

        //            // �v���C���[���R�Ȃ�Ɉړ�������
        //            LaunchPlayer(playerRb, checkPoint);
        //        }
        //    }
        //}

      


        private IEnumerator DelayStart()
        {
            rb.isKinematic = true;
            yield return new WaitUntil(() => GameManager.I.canPlay);
            yield return new WaitForSeconds(delayStart);
            rb.isKinematic = false;

        }
    }
}
