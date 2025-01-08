using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingFloor : MonoBehaviour
{
    public float tiltSpeed = 5.0f;         // 傾く速度
    public float maxTiltAngle = 30.0f;    // 最大傾き角度
    public float resetSpeed = 2.0f;       // 元に戻る速度

    private Quaternion initialRotation;   // 初期の床の回転
    private Transform player;             // プレイヤーのTransform
    private bool isPlayerOnFloor = false; // プレイヤーが床に乗っているかどうか

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが床に乗った場合
        if (collision.collider.CompareTag("Ball"))
        {
            player = collision.transform;
            isPlayerOnFloor = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // プレイヤーが床から離れた場合
        if (collision.collider.CompareTag("Ball"))
        {
            isPlayerOnFloor = false;
            player = null;
        }
    }

    void Update()
    {
        if (isPlayerOnFloor && player != null)
        {
            // プレイヤーの位置に応じて床を傾ける
            Vector3 localPlayerPosition = transform.InverseTransformPoint(player.position);
            float tiltX = Mathf.Clamp(localPlayerPosition.z, -1, 1) * maxTiltAngle;
            float tiltZ = -Mathf.Clamp(localPlayerPosition.x, -1, 1) * maxTiltAngle;

            Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ) * initialRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
        else
        {
            // プレイヤーがいない場合は元に戻る
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, resetSpeed * Time.deltaTime);
        }
    }
}
