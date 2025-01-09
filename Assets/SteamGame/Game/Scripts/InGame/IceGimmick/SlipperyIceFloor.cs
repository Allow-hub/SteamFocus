using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlipperyIceFloor : MonoBehaviour
{
    public float slipForce = 10f;  // 滑る力
    public float dragOnIce = 1f;   // 氷の上のドラッグ値

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // プレイヤーが氷の床に触れた場合、Rigidbodyのドラッグを変更して滑りやすくする
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.drag = dragOnIce;  // 氷の上で滑るようにドラッグを設定
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // プレイヤーが氷の床にいる間、滑る力を加える
                Debug.Log("OnCollisionStay: Applying slip force");
                Vector3 slipDirection = playerRb.velocity.normalized;
                playerRb.AddForce(slipDirection * slipForce, ForceMode.Force);
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが氷の床から離れたら、ドラッグを元に戻す
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.drag = 0;  // 普通の床と同じように戻す
            }
        }
    }
}
