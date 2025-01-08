using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingLog : MonoBehaviour
{
    public Transform[] waypoints;  // 経路となる目印のリスト
    public float moveSpeed = 5f;   // 丸太の移動速度
    public float rotationSpeed = 5f; // 回転速度

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 現在の目印に向かって移動
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // 回転（丸太が進行方向に合わせて回転）
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 次の目印に進む
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // 経路の最後に到達した場合
                currentWaypointIndex = 0; // 最初からやり直す（または停止）
            }
        }
    }
}
