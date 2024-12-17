using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand : MonoBehaviour
{
    public float sinkSpeed = 0.5f;  // プレイヤーが沈む速度
    public float increasedDrag = 20.0f;  // 流砂内での空気抵抗
    private Rigidbody playerRigidbody;  // プレイヤーのRigidbodyへの参照
    private bool isPlayerInQuicksand = false;

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが流砂に入ったとき
        if (other.CompareTag("Ball"))
        {
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                isPlayerInQuicksand = true;
                playerRigidbody.drag = increasedDrag;  // 空気抵抗を増加させる
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが流砂から出たとき
        if (other.CompareTag("Ball") && playerRigidbody != null)
        {
            isPlayerInQuicksand = false;
            playerRigidbody.drag = 0.0f;  // 空気抵抗を元に戻す
            playerRigidbody = null;
        }
    }

    void Update()
    {
        // プレイヤーが流砂内にいる場合、徐々にY軸方向に沈める
        if (isPlayerInQuicksand && playerRigidbody != null)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + Vector3.down * sinkSpeed * Time.deltaTime);
        }
    }
}
