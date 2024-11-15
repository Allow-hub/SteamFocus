using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public float riseSpeed = 1f;  // ���ǂ��㏸����X�s�[�h
    private bool isRising = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;  // ���ǂ̌��̈ʒu���L�^
    }

    void Update()
    {
        if (isRising)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;  // ���ǂ��㏸
        }
    }

    public void StartRising()
    {
        isRising = true;  // ���ǂ��㏸������
    }

    public void StopRising()
    {
        isRising = false;  // ���ǂ̏㏸���~
        transform.position = originalPosition;  // ���̈ʒu�ɖ߂�
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // ���ǂɓ�����Ə��������グ��
            }
        }
    }
}
