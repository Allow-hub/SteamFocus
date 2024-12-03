using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    public Vector3 slideDirection = Vector3.right; // スライドする方向
    public float slideDistance = 5.0f;             // 壁がスライドする距離
    public float moveSpeed = 3.0f;                 // スライドの速度
    public float pauseTime = 2.0f;                 // 壁が停止する時間

    private Vector3 initialPosition;
    private bool isMovingOut = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isMovingOut)
        {
            // 壁がスライドする動き
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + slideDirection * slideDistance, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition + slideDirection * slideDistance) < 0.1f)
            {
                isMovingOut = false;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
        else
        {
            // 壁が元に戻る動き
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                isMovingOut = true;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
    }

    void SwitchDirection()
    {
        isMovingOut = !isMovingOut;
    }
}
