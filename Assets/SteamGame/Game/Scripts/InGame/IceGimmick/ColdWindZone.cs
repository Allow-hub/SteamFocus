using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWindZone : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(-1, 0, 0);  // 風の方向
    public float windStrength = 10f;                       // 風の強さ

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.AddForce(windDirection.normalized * windStrength, ForceMode.Force);
            }
        }
    }
}
