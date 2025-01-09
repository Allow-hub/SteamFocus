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
        [SerializeField] private float moveSpeed = 5f;            // 基本移動速度
        [SerializeField] private float rotationSpeed = 2f;        // 回転速度
        [SerializeField] private float limit = 1.2f;             // ボールの速度の何倍まで許容するか
        [SerializeField] private float decelerationFactor = 2f;   // 減速の強さ
        [SerializeField] private Rigidbody ballRb;
        private const string walkAnimName = "IsWalking";

        private Camera playerCamera;

        [Header("Attack")]
        [SerializeField] private float forwardForce = 10f;        // 攻撃時の前方力
        [SerializeField] private float upwardForce = 5f;          // 攻撃時の上方力
        [SerializeField] private float attackCoolTime = 1f;       // 攻撃のクールタイム
        [SerializeField] private float attackTime = 1f;           // アタック後に待機する時間
        [SerializeField] private float teamAttackForwardForce;
        [SerializeField] private float teamAttackUpwardForce;

        private const string attackAnimName = "IsAttacking";
        private bool canAttack = true;
        private bool isTaking = false;                             // アタック中フラグ

        private float initForwardForce;
        private float initUpwardForce;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 3f;            // ジャンプ力
        [SerializeField] private float jumpForwardForce = 100f; 
        [SerializeField] private float jumpCoolTime = 1f;         // ジャンプのクールタイム
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
                // ジャンプやタックルの処理
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
                // 物理演算を使った移動処理
                //HandleMovement();
                //LimitSpeed();
            }
        }

        private void HandleMovement()
        {
            // 入力に基づく移動ベクトルを計算
            Vector3 inputVector = playerInput.InputVector;
            Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.z).normalized * moveSpeed;

            if (inputVector == Vector3.zero)
            {
                // 止まっている場合、移動アニメーションを停止
                anim.SetBool(walkAnimName, false);
                return;
            }

            // カメラの向きに基づいて移動ベクトルを調整
            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 adjustedMovement = (cameraForward * inputVector.z + cameraRight * inputVector.x).normalized * moveSpeed;

            // プレイヤーの移動
            // 他の物理演算との干渉を防ぐために座標操作で移動させている
            rb.MovePosition(rb.position + adjustedMovement * Time.deltaTime);
            
            // プレイヤーを移動方向に向ける
            if (adjustedMovement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 移動中はアニメーションを発火
            anim.SetBool(walkAnimName, true);
        }

        private void LimitSpeed()
        {
            float ballSpeed = ballRb.velocity.magnitude;
            if (ballSpeed == 0) return;  // ボールが停止している場合、計算を行わない

            float speedRatio = rb.velocity.magnitude / (ballSpeed * limit);

            // speedRatioがNaNまたは無限大にならないようにチェック
            if (float.IsNaN(speedRatio) || float.IsInfinity(speedRatio) || speedRatio <= 1)
            {
                return;  // 無効な値の場合は処理をスキップ
            }
            // プレイヤーの速度がボールの速度のn倍を超えている場合
            Vector3 reverseForce = -rb.velocity.normalized * moveSpeed * (speedRatio - 1);

            // 減速の強さを調整する
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
                isTaking = true;  // アタック中フラグを立てる
                anim.SetTrigger(attackAnimName);  
                StartCoroutine(CoolTime(false));
                StartCoroutine(AttackDelay());  // アタック後の遅延を開始
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
            // ジャンプの力
            Vector3 jumpForceVector = Vector3.up * jumpForce;

            // 前方向の力
            Vector3 forwardForceVector = transform.forward * jumpForwardForce;
            // 合成した力を加える
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
            // アタック後、指定時間待機
            yield return new WaitForSeconds(attackTime);
            isTaking = false;  // アタック終了
        }

        //public void Break(Vector3 checkPoint)
        //{
        //    // プレイヤーオブジェクトを全取得
        //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        Rigidbody playerRb = players[i].GetComponent<Rigidbody>();

        //        if (playerRb != null)
        //        {
        //            Debug.Log(players.Length);

        //            // プレイヤーを山なりに移動させる
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
