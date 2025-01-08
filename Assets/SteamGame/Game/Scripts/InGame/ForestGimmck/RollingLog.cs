using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingLog : MonoBehaviour
{
    public Transform[] waypoints;  // �o�H�ƂȂ�ڈ�̃��X�g
    public float moveSpeed = 5f;   // �ۑ��̈ړ����x
    public float rotationSpeed = 5f; // ��]���x

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // ���݂̖ڈ�Ɍ������Ĉړ�
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // �ړ�
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // ��]�i�ۑ����i�s�����ɍ��킹�ĉ�]�j
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ���̖ڈ�ɐi��
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // �o�H�̍Ō�ɓ��B�����ꍇ
                currentWaypointIndex = 0; // �ŏ������蒼���i�܂��͒�~�j
            }
        }
    }
}
