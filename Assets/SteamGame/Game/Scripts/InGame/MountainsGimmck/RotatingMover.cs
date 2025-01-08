using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBetweenPoints : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA;          // �ړ��J�n�_
    [SerializeField] private Transform pointB;          // �ړ��I���_
    [SerializeField] private float moveSpeed = 2.0f;    // �ړ����x
    [SerializeField] private float rotationSpeed = 90.0f; // ��]���x�i�x/�b�j
    [SerializeField] private Transform rotationCenter;  // ��]�̒��S�ƂȂ��̃I�u�W�F�N�g

    private bool movingToB = true; // ���݂̈ړ���itrue: Point B, false: Point A�j

    void Update()
    {
        // �ړ����Ȃ����]
        MoveBetweenPoints();
        RotateObject();
    }

    private void RotateObject()
    {
        // ��]�̒��S���w�肵�ĉ�]������
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
        // ���݂̃^�[�Q�b�g�n�_
        Transform target = movingToB ? pointB : pointA;

        // �^�[�Q�b�g�n�_�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // �^�[�Q�b�g�n�_�ɓ��B����������𔽓]���A��]���S�𔽓]������
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;

            // ��]���S�𔽓]�i���W�𔽓]������j
            if (rotationCenter != null)
            {
                // ���݂̉�]���S�𔽓]������
                rotationCenter.position = (rotationCenter.position == pointA.position) ? pointB.position : pointA.position;
            }
        }
    }
}
