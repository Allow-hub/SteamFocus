using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulum : MonoBehaviour
{
    public float swingSpeed = 2.0f;    // �h��鑬��
    public float swingAngle = 45.0f;  // �ő�p�x
    public Vector3 swingAxis = Vector3.forward; // �h��鎲�����R�ɐݒ�
    public Transform attachPoint;     // �{�[�����Ԃ牺����ʒu

    private Quaternion initialRotation;

    void Start()
    {
        // ������]��ۑ�
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // ���ԂɊ�Â��ău�����R��h�炷
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        // �w�肳�ꂽ������ɉ�]
        Quaternion swingRotation = Quaternion.AngleAxis(angle, swingAxis);

        // ������]����ɗh��铮����K�p
        transform.localRotation = initialRotation * swingRotation;
    }

    public Transform GetAttachPoint()
    {
        return attachPoint; // �{�[�����Ԃ牺����ʒu���
    }
}
