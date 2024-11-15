using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public float riseSpeed = 1f;  // 砂壁が上昇するスピード
    private bool isRising = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;  // 砂壁の元の位置を記録
    }

    void Update()
    {
        if (isRising)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;  // 砂壁が上昇
        }
    }

    public void StartRising()
    {
        isRising = true;  // 砂壁を上昇させる
    }

    public void StopRising()
    {
        isRising = false;  // 砂壁の上昇を停止
        transform.position = originalPosition;  // 元の位置に戻す
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // 砂壁に当たると少し押し上げる
            }
        }
    }
}
