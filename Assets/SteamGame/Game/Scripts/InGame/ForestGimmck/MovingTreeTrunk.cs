using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTreeTrunk : MonoBehaviour
{
    public float extendDistance = 5.0f;   // �����L�т鋗��
    public float moveSpeed = 2.0f;        // �����L�яk�݂��鑬�x
    public float pauseTime = 1.0f;        // �����L�ѐ؂��Ă���߂�܂ł̎���

    private Vector3 initialPosition;
    private bool isExtending = true;

    void Start()
    {
        // �����ʒu���L�^
        initialPosition = transform.position;
    }

    void Update()
    {
        // ����L�΂�����k�߂��肷�铮��
        if (isExtending)
        {
            // �L�т铮��
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + transform.forward * extendDistance, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition + transform.forward * extendDistance) < 0.1f)
            {
                isExtending = false;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
        else
        {
            // ���̈ʒu�ɖ߂铮��
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                isExtending = true;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
    }

    void SwitchDirection()
    {
        isExtending = !isExtending;
    }
}
