using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuicksandZone : MonoBehaviour
{
    public float sinkSpeed = 0.1f;  // プレイヤーが沈む速度
    private bool isPlayerInQuicksand = false;
    private Transform playerTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInQuicksand = true;
            playerTransform = other.transform;  // プレイヤーのTransformを取得
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInQuicksand = false;
            playerTransform = null;  // プレイヤーがエリアから出ると沈むのを止める
        }
    }

    void Update()
    {
        if (isPlayerInQuicksand && playerTransform != null)
        {
            // プレイヤーを下方向に徐々に移動させる
            playerTransform.position += Vector3.down * sinkSpeed * Time.deltaTime;
        }
    }
}
