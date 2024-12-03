using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class RockClimbingManager : MonoBehaviour
{
    [Header("Moving Platforms")]
    public Transform[] platforms;    // 動く足場
    public float moveSpeed = 2f;     // 足場の移動速度
    public float moveRange = 2f;     // 足場の移動範囲

    [Header("Player Interaction")]
    public Transform player;         // プレイヤー
    public float pushForce = 10f;    // プレイヤーを吹き飛ばす力
    public Vector3 pushDirection = Vector3.back; // 吹き飛ばす方向

    private Vector3[] initialPositions;

    void Start()
    {
        // 足場の初期位置を記録
        initialPositions = new Vector3[platforms.Length];
        for (int i = 0; i < platforms.Length; i++)
        {
            initialPositions[i] = platforms[i].position;
        }
    }

    void Update()
    {
        MovePlatforms();
    }

    void MovePlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 startPos = initialPositions[i];
            platforms[i].position = startPos + new Vector3(0, Mathf.Sin(Time.time * moveSpeed) * moveRange, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 force = pushDirection.normalized * pushForce;
                playerRb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
