using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePathMover : MonoBehaviour
{
    public Vector3[] waypoints; // ���񂷂�|�C���g�i�l�p�`�̒��_��ݒ�j
    public float moveSpeed = 2.0f; // �ړ����x
    [SerializeField]private int currentWaypointIndex = 0;

    void Start()
    {
        // �����ʒu���ŏ��̃E�F�C�|�C���g�ɐݒ�
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0];
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // ���݂̃E�F�C�|�C���g�Ɍ������Ĉړ�
        Vector3 target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // �E�F�C�|�C���g�ɓ��B�����玟�̃E�F�C�|�C���g��
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // ���[�v����
        }
    }
}
