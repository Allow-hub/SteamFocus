using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulum : MonoBehaviour
{
    public float swingSpeed = 2.0f;  // �h��鑬��
    public float swingAngle = 45.0f; // �ő�p�x

    private float initialRotation;

    void Start()
    {
        initialRotation = transform.localEulerAngles.z;
    }

    void Update()
    {
        // ���ԂɊ�Â��ău�����R��h�炷
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localEulerAngles = new Vector3(0, 0, initialRotation + angle);
    }
}
