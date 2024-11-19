using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FallenWoodGimmick : MonoBehaviour
    {
        [SerializeField] GameObject wood;
        Rigidbody rb;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "Player")
            {
                rb = wood.GetComponent<Rigidbody>();
                rb.velocity = transform.right.normalized * 1;
            }
        }
    }
}
