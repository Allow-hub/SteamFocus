using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyIceFloor : MonoBehaviour
{
    public float slipForce = 10f;  // ŠŠ‚é—Í

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 slipDirection = playerRb.velocity.normalized;
                playerRb.AddForce(slipDirection * slipForce, ForceMode.Force);
            }
        }
    }
}
