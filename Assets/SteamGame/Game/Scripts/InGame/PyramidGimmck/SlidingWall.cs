using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    public Vector3 slideDirection = Vector3.right; // �X���C�h�������
    public float slideDistance = 5.0f;             // �ǂ��X���C�h���鋗��
    public float moveSpeed = 3.0f;                 // �X���C�h�̑��x
    public float pauseTime = 2.0f;                 // �ǂ���~���鎞��

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
            // �ǂ��X���C�h���铮��
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + slideDirection * slideDistance, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition + slideDirection * slideDistance) < 0.1f)
            {
                isMovingOut = false;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
        else
        {
            // �ǂ����ɖ߂铮��
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
