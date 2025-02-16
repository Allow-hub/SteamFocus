using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandFlow : MonoBehaviour
{
    public Vector3 flowDirection = new Vector3(1, 0, 0); // 流れの方向
    public float flowStrength = 5.0f; // 流れの強さ

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 流れの方向に力を加える
                playerRb.AddForce(flowDirection.normalized * flowStrength, ForceMode.Acceleration);
            }
        }
    }
}
