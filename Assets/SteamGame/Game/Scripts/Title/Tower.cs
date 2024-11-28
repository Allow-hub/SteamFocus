using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateDirection= Vector3.right;
        [SerializeField] private float rotateSpeed = 1;

        private void Update ()
        {
            transform.Rotate(rotateDirection*rotateSpeed);  
        }

    }
}
