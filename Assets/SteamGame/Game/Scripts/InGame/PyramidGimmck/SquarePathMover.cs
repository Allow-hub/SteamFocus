using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePathMover : MonoBehaviour
{
    public Vector3[] waypoints; // 巡回するポイント（四角形の頂点を設定）
    public float moveSpeed = 2.0f; // 移動速度
    [SerializeField]private int currentWaypointIndex = 0;

    void Start()
    {
        // 初期位置を最初のウェイポイントに設定
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0];
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 現在のウェイポイントに向かって移動
        Vector3 target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // ウェイポイントに到達したら次のウェイポイントへ
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // ループ処理
        }
    }
}
