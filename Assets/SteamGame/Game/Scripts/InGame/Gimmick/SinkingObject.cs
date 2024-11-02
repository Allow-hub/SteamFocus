using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingObject : MonoBehaviour
{
    public float speed = 5.0f; // 移動スピード
    public float jumpPower = 5.0f; // ジャンプ力
    public float sinkSpeed = 0.5f; // 流砂での沈む速度
    private bool isJumping = false; // ジャンプ判定
    private bool isInQuicksand = false; // 流砂エリア判定
    private Rigidbody playerRigidbody;

    void Start()
    {
        // Rigidbodyが存在するか確認し、なければ追加
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            playerRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        playerRigidbody.drag = 0.0f; // 空気抵抗
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // キャラクターを回転させない
    }

    void Update()
    {
        // 通常の移動処理
        if (Input.GetKey(KeyCode.UpArrow)) // 上矢印キーで奥へ移動
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow)) // 下矢印キーで手前へ移動
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow)) // 右矢印キーで右へ移動
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) // 左矢印キーで左へ移動
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        // ジャンプ処理（流砂内では無効）
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isInQuicksand == false)
        {
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumping = true; // ジャンプ中はジャンプさせない
        }

        // 流砂内にいる場合、プレイヤーを徐々に下に移動させる
        if (isInQuicksand)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false; // FloorのTagのオブジェクトに着地したらジャンプ可能にする
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 流砂エリアに入ったら空気抵抗を増やして沈む状態にする
        if (collider.gameObject.CompareTag("QuickSand"))
        {
            isInQuicksand = true;
            isJumping = false; // 流砂内ではジャンプできない
            playerRigidbody.drag = 20.0f; // 空気抵抗を増加させて動きを鈍くする
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // 流砂エリアから出たら空気抵抗を元に戻し、通常の状態に戻す
        if (collider.gameObject.CompareTag("QuickSand"))
        {
            isInQuicksand = false;
            playerRigidbody.drag = 0.0f; // 空気抵抗を元に戻す
        }
    }
}
