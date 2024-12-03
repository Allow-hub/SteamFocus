using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSwamp : MonoBehaviour
{
    public Transform respawnPoint;   // ボールのリスポーン地点
    public float playerOffsetY = 1.0f; // プレイヤーをボールの上に配置する高さのオフセット

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // ボールとプレイヤーをリスポーン
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーを検索
            if (player != null)
            {
                StartCoroutine(RespawnBallAndPlayer(other.transform, player.transform));
            }
        }
    }

    IEnumerator RespawnBallAndPlayer(Transform ball, Transform player)
    {
        // 1秒の遅延を追加
        yield return new WaitForSeconds(1f);

        // ボールをリスポーン地点に移動
        ball.position = respawnPoint.position;
        ball.rotation = respawnPoint.rotation;

        // プレイヤーをボールの上部に配置
        Vector3 playerRespawnPosition = ball.position + Vector3.up * playerOffsetY;
        player.position = playerRespawnPosition; // プレイヤーの位置をボールと同期
        player.rotation = ball.rotation;        // プレイヤーの回転をボールに同期

        Debug.Log("Ball and Player have been respawned at the correct location.");
    }
}
