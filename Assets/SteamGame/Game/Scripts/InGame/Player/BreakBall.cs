using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class BreakBall : MonoBehaviour
    {
        [SerializeField] private float duration = 3f;

        private void OnCollisionEnter(Collision collision)
        {
            GameManager.I.BreakPlayer(duration);   
        }
        private void OnTriggerEnter(Collider other)
        {
            GameManager.I.BreakPlayer(duration);
        }
    }
}
