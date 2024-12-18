using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    ///���� Transform�^�͎Q�ƌ^�Ȃ̂ŏ������Ɏg���Ȃ�
    /// </summary>
    public class RepairOutWall : MonoBehaviour
    {
        private Vector3 initialPosition;   // �����ʒu��ۑ�
        private Quaternion initialRotation; // ������]��ۑ�
        private Rigidbody rb;
        private void Awake()
        {
            // ������Ԃ�ۑ�
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            rb=GetComponent<Rigidbody>();   
        }

        private void OnEnable()
        {
            // ������ԂɃ��Z�b�g
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
        }
    }
}
