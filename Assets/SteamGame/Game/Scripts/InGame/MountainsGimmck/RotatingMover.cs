using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBetweenPoints : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA;          // 移動開始点
    [SerializeField] private Transform pointB;          // 移動終了点
    [SerializeField] private float moveSpeed = 2.0f;    // 移動速度
    [SerializeField] private float rotationSpeed = 90.0f; // 回転速度（度/秒）
    [SerializeField] private Transform rotationCenter;  // 回転の中心となる空のオブジェクト

    private bool movingToB = true; // 現在の移動先（true: Point B, false: Point A）

    void Update()
    {
        // 移動しながら回転
        MoveBetweenPoints();
        RotateObject();
    }

    private void RotateObject()
    {
        // 回転の中心を指定して回転させる
        if (rotationCenter != null)
        {
            transform.RotateAround(rotationCenter.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Rotation center is not assigned.");
        }
    }

    private void MoveBetweenPoints()
    {
        // 現在のターゲット地点
        Transform target = movingToB ? pointB : pointA;

        // ターゲット地点に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // ターゲット地点に到達したら方向を反転し、回転中心を反転させる
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;

            // 回転中心を反転（座標を反転させる）
            if (rotationCenter != null)
            {
                // 現在の回転中心を反転させる
                rotationCenter.position = (rotationCenter.position == pointA.position) ? pointB.position : pointA.position;
            }
        }
    }
}
