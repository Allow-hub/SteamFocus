using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandFlow : MonoBehaviour
{
    public Vector3 flowDirection = new Vector3(1, 0, 0); // —¬‚ê‚Ì•ûŒü
    public float flowStrength = 5.0f; // —¬‚ê‚Ì‹­‚³

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // —¬‚ê‚Ì•ûŒü‚É—Í‚ð‰Á‚¦‚é
                playerRb.AddForce(flowDirection.normalized * flowStrength, ForceMode.Acceleration);
            }
        }
    }
}
