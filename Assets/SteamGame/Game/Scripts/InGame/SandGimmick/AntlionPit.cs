using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionPit : MonoBehaviour
{
    public float pullStrength = 5.0f;       // プレイヤーを中心に引き寄せる力の強さ
    public float sinkSpeed = 0.2f;          // プレイヤーが沈む速度
    public float fallDepth = -5.0f;         // 落下が始まるY座標の深さ
    private Transform pitCenter;            // 蟻地獄の中心位置
    private Rigidbody playerRigidbody;      // プレイヤーのRigidbodyへの参照
    private bool isPlayerInPit = false;

    void Start()
    {
        // 蟻地獄の中心位置を設定（オブジェクトの中心）
        pitCenter = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが蟻地獄に入ったとき
        if (other.CompareTag("Player"))
        {
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                isPlayerInPit = true;
                playerRigidbody.drag = 5.0f;  // 流砂のように空気抵抗を増加させて動きを鈍くする
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが蟻地獄から出たとき
        if (other.CompareTag("Player") && playerRigidbody != null)
        {
            isPlayerInPit = false;
            playerRigidbody.drag = 0.0f;  // 空気抵抗を元に戻す
            playerRigidbody = null;
        }
    }

    void Update()
    {
        // プレイヤーが蟻地獄内にいる場合
        if (isPlayerInPit && playerRigidbody != null)
        {
            // 1. 中心への引き寄せ
            Vector3 directionToCenter = (pitCenter.position - playerRigidbody.position).normalized;
            float distance = Vector3.Distance(playerRigidbody.position, pitCenter.position);
            float adjustedPullStrength = pullStrength / Mathf.Max(1.0f, distance); // 中心に近づくほど強く引き寄せる
            playerRigidbody.AddForce(directionToCenter * adjustedPullStrength, ForceMode.Acceleration);

            // 2. その場での沈み
            playerRigidbody.MovePosition(playerRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);

            // 3. 一定の深さに達したら落下させる
            if (playerRigidbody.position.y <= fallDepth)
            {
                playerRigidbody.drag = 0.0f;  // 空気抵抗を元に戻す
                playerRigidbody.AddForce(Vector3.down * 10.0f, ForceMode.VelocityChange);  // 強制的に落下させる
                isPlayerInPit = false;  // 落下後に沈む処理を止める
            }
        }
    }
}
