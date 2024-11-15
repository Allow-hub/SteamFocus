using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TechC
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LaserController laserController;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                laserController.TouchLaser(other.gameObject);
            }
        }
    }
}
