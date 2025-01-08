using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class NormalTree : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                if (SeManager.I == null) return;
                SeManager.I.PlaySe(0, 0);
            }
        }
    }
}
