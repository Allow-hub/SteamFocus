using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulum : MonoBehaviour
{
    public float swingSpeed = 2.0f;    // 揺れる速さ
    public float swingAngle = 45.0f;  // 最大角度
    public Vector3 swingAxis = Vector3.forward; // 揺れる軸を自由に設定
    public Transform attachPoint;     // ボールがぶら下がる位置

    private Quaternion initialRotation;

    void Start()
    {
        // 初期回転を保存
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // 時間に基づいてブランコを揺らす
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        // 指定された軸を基準に回転
        Quaternion swingRotation = Quaternion.AngleAxis(angle, swingAxis);

        // 初期回転を基準に揺れる動きを適用
        transform.localRotation = initialRotation * swingRotation;
    }

    public Transform GetAttachPoint()
    {
        return attachPoint; // ボールがぶら下がる位置を提供
    }
}
