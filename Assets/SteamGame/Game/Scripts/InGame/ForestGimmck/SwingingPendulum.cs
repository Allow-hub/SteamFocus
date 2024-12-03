using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulum : MonoBehaviour
{
    public float swingSpeed = 2.0f;  // 揺れる速さ
    public float swingAngle = 45.0f; // 最大角度
    public Transform attachPoint;   // ボールがぶら下がる位置

    private float initialRotation;

    void Start()
    {
        initialRotation = transform.localEulerAngles.z;
    }

    void Update()
    {
        // 時間に基づいてブランコを揺らす
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localEulerAngles = new Vector3(0, 0, initialRotation + angle);
    }

    public Transform GetAttachPoint()
    {
        return attachPoint; // ボールがぶら下がる位置を提供
    }
}
