using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWindZone : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(-1, 0, 0);  // ���̕���
    public float windStrength = 10f;                       // ���̋���

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
