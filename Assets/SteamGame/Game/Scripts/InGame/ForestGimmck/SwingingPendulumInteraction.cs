using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;

public class SwingingPendulumInteraction : MonoBehaviour
{
    public Transform attachPoint;          // ボールをぶら下げる位置
    public float attachSpeed = 5f;         // ボールがattachPointに向かう速度
    public float reattachCooldown = 1.0f;  // 再接触を防ぐクールダウン時間
    public KeyCode jumpKey = KeyCode.Space; // ジャンプキー
    public float jumpForce = 500f;         // ジャンプ時に加える力

    private Transform currentBall;         // 現在つかんでいるボール
    private GameObject player;             // プレイヤーオブジェクト
    private bool isSwinging = false;       // ボールがぶら下がっているか
    private bool canAttach = true;         // 再接触可能かどうか

    void Start()
    {
        // プレイヤーをシーンから取得
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canAttach || !other.CompareTag("Ball")) return;

        AttachToSwing(other.transform);
    }

    void Update()
    {
        // ジャンプキーでボールを離す
        if (isSwinging && Input.GetKeyDown(jumpKey))
        {
            DetachFromSwing();
        }

        // ボールをスムーズにattachPointに移動
        if (isSwinging && currentBall != null)
        {
            SmoothAttach();
        }
    }

    void AttachToSwing(Transform ball)
    {
        if (isSwinging) return;

        isSwinging = true;
        currentBall = ball;

        // 物理挙動を一時停止
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.isKinematic = true;
        }

        Debug.Log("Ball attached to the Swing.");
    }

    void SmoothAttach()
    {
        // ボールをスムーズに移動
        currentBall.position = Vector3.Lerp(currentBall.position, attachPoint.position, attachSpeed * Time.deltaTime);

        // プレイヤーの位置をボールに同期
        if (player != null)
        {
            player.transform.position = currentBall.position;
        }

        // 完全にattachPointに到達した場合
        if (Vector3.Distance(currentBall.position, attachPoint.position) < 0.1f)
        {
            currentBall.SetParent(attachPoint); // ボールをブランコの子オブジェクトに設定
        }
    }

    void DetachFromSwing()
    {
        if (!isSwinging) return;

        isSwinging = false;

        // 親子関係を解除
        if (currentBall != null)
        {
            currentBall.SetParent(null);

            // 物理挙動を再有効化
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = false;

                // ジャンプ方向を計算
                Vector3 jumpDirection = CalculateJumpDirection();
                ballRb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            }
        }

        Debug.Log("Ball detached from the Swing.");

        // 再接触を防ぐクールダウンを開始
        StartCoroutine(ReattachCooldown());
    }

    Vector3 CalculateJumpDirection()
    {
        // ブランコの現在の速度を取得
        Rigidbody swingRb = GetComponent<Rigidbody>();
        Vector3 swingVelocity = swingRb != null ? swingRb.velocity : Vector3.zero;

        // ジャンプ方向を決定
        Vector3 jumpDirection = (swingVelocity + Vector3.up).normalized;
        return jumpDirection;
    }

    IEnumerator ReattachCooldown()
    {
        canAttach = false; // 一時的に再接触を禁止
        yield return new WaitForSeconds(reattachCooldown);
        canAttach = true;  // 再接触を許可
    }
}
