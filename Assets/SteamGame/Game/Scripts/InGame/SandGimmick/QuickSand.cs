using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand : MonoBehaviour
{
    public float sinkSpeed = 0.5f;  // プレイヤーが沈む速度
    public float increasedDrag = 20.0f;  // 流砂内での空気抵抗
    public float jumpBoost = 10.0f;  // ジャンプ力を増加させる力
    private Rigidbody ballRigidbody;  // プレイヤーのRigidbodyへの参照
    private bool isPlayerInQuicksand = false;

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが流砂に入ったとき
        if (other.CompareTag("Ball"))
        {
            ballRigidbody = other.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                isPlayerInQuicksand = true;
                ballRigidbody.drag = increasedDrag;  // 空気抵抗を増加させる
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが流砂から出たとき
        if (other.CompareTag("Ball") && ballRigidbody != null)
        {
            isPlayerInQuicksand = false;
            ballRigidbody.drag = 0.0f;  // 空気抵抗を元に戻す
            ballRigidbody = null;
        }
    }

    void Update()
    {
        if (isPlayerInQuicksand && ballRigidbody != null)
        {
            // プレイヤーを徐々にY軸方向に沈める
            //ballRigidbody.MovePosition(ballRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);

            // ジャンプ時にジャンプ力を増加させる
            if (Input.GetButtonDown("Jump"))
            {
                ballRigidbody.AddForce(Vector3.up * jumpBoost, ForceMode.Impulse);
            }
        }
    }
}
