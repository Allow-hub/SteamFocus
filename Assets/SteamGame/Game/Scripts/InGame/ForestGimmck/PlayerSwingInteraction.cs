using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class PlayerSwingInteraction : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space; // ジャンプキー
    public float jumpForce = 500f;          // ジャンプ時に加える力
    public float attachSpeed = 5f;         // ボールがattachPointに向かう速度
    public float reattachCooldown = 1.0f;  // 再接触を防ぐクールダウン時間

    private Rigidbody containerRb;         // ボールのRigidbody
    private Transform currentSwing;        // 現在のブランコ
    private bool isSwinging = false;       // ボールがブランコにぶら下がっているか
    private bool canAttach = true;         // 再接触可能かどうか
    private Transform attachPoint;         // ブランコのぶら下がりポイント
    private GameObject player;             // プレイヤーオブジェクト

    void Start()
    {
        // ボールのRigidbodyを取得
        containerRb = GetComponent<Rigidbody>();
        if (containerRb == null)
        {
            Debug.LogError("PlayerSwingInteraction: Rigidbody not found on BallContainer!");
        }

        // プレイヤーをシーンから取得
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canAttach || !other.CompareTag("Swing")) return;

        AttachToSwing(other.transform);
    }

    void Update()
    {
        // ジャンプキーでブランコから離れる
        if (isSwinging && Input.GetKeyDown(jumpKey))
        {
            DetachFromSwing();
        }

        // ボールをスムーズにattachPointに移動
        if (isSwinging && attachPoint != null)
        {
            SmoothAttach();
        }
    }

    void AttachToSwing(Transform swing)
    {
        if (isSwinging) return;

        isSwinging = true;
        currentSwing = swing;

        SwingingPendulum pendulum = swing.GetComponent<SwingingPendulum>();
        if (pendulum != null)
        {
            attachPoint = pendulum.GetAttachPoint();

            // 物理挙動を一時停止
            containerRb.isKinematic = true;

            Debug.Log("BallContainer attached to Swing.");
        }
    }

    void SmoothAttach()
    {
        // ボールをスムーズに移動
        transform.position = Vector3.Lerp(transform.position, attachPoint.position, attachSpeed * Time.deltaTime);

        // プレイヤーの位置をボールに同期
        if (player != null)
        {
            player.transform.position = transform.position;
        }

        // 完全にattachPointに到達した場合
        if (Vector3.Distance(transform.position, attachPoint.position) < 0.1f)
        {
            transform.SetParent(attachPoint); // ボールをブランコの子オブジェクトに設定
            attachPoint = null;              // 移動処理を終了
        }
    }

    void DetachFromSwing()
    {
        if (!isSwinging) return;

        isSwinging = false;

        // 親子関係を解除
        transform.SetParent(null);
        containerRb.isKinematic = false; // 重力を再有効化

        // ジャンプ方向を計算
        Vector3 jumpDirection = CalculateJumpDirection();
        containerRb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

        Debug.Log($"BallContainer detached from Swing. Jump direction: {jumpDirection}");

        // 再接触を防ぐクールダウンを開始
        StartCoroutine(ReattachCooldown());
    }

    Vector3 CalculateJumpDirection()
    {
        // ブランコの速度（動きの勢い）を取得
        Rigidbody swingRb = currentSwing.GetComponent<Rigidbody>();
        Vector3 swingVelocity = swingRb != null ? swingRb.velocity : Vector3.zero;

        // ボールの現在の前方向を取得（進行方向）
        Vector3 ballForward = transform.forward;

        // ブランコの回転から前方向を計算
        Vector3 swingForward = currentSwing.rotation * Vector3.forward;

        // X方向を強調した方向補正
        float xBias = 1.5f; // X方向の比率を強調する
        swingForward.x *= xBias;
        ballForward.x *= xBias;

        // -Z方向のバイアスを追加
        float zBias = -0.5f; // Z方向を少し引き寄せる
        swingForward.z += zBias;
        ballForward.z += zBias;

        // 最終的なジャンプ方向を計算（揺れの勢い + ブランコの方向）
        Vector3 combinedDirection = (swingVelocity + swingForward + ballForward).normalized;

        // X成分やZ成分を調整
        combinedDirection.x *= 1.2f;  // X成分の補正
        combinedDirection.z *= 1.1f;  // Z成分の補正（調整しつつ-方向を維持）

        // 水平方向を維持しつつ、上方向を追加
        combinedDirection += Vector3.up * 0.5f;

        return combinedDirection.normalized;
    }


    System.Collections.IEnumerator ReattachCooldown()
    {
        canAttach = false; // 一時的に再接触を禁止
        yield return new WaitForSeconds(reattachCooldown);
        canAttach = true;  // 再接触を許可
    }
}
