using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandFlow : MonoBehaviour
{
    public Vector3 flowDirection = new Vector3(1, 0, 0); // ����̕���
    public float flowStrength = 5.0f; // ����̋���

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // ����̕����ɗ͂�������
                playerRb.AddForce(flowDirection.normalized * flowStrength, ForceMode.Acceleration);
            }
        }
    }
}
