using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class BarCollider : MonoBehaviour
    {
        private PlayerController playerController;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                if(playerController==null)
                    playerController = collision.gameObject.GetComponent<PlayerController>();
                //playerController.Break();
            }
        }
    }
}
