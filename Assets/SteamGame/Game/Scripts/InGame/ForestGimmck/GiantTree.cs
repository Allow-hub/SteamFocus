using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantTree : MonoBehaviour
{
    public float moveSpeed = 5f;  // 木の上を進むスピード

    private bool isPlayerOnTree = false;  // プレイヤーが木に乗っているか

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTree = true;  // プレイヤーが木に乗った
            other.transform.SetParent(transform);  // プレイヤーを木の親に設定して一緒に動かす
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTree = false;  // プレイヤーが木から降りた
            other.transform.SetParent(null);  // 親設定を解除
        }
    }

    void Update()
    {
        if (isPlayerOnTree)
        {
            // プレイヤーが木に乗っている場合、木と一緒に進む
            float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * move);
        }
    }
}
