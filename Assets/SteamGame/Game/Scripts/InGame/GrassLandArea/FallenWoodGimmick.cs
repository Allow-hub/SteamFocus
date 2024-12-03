using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FallenWoodGimmick : MonoBehaviour
    {
        [SerializeField]�@private GameObject pole;
        private Rigidbody rb;
        private bool isFallen = false; // �|�ꂽ���ǂ���

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isFallen)
            {
                rb = pole.GetComponent<Rigidbody>();
                rb.AddTorque(transform.forward * 20f, ForceMode.Impulse);
                isFallen = true;
                //StartCoroutine(StopMove());
            }
        }

        /*IEnumerator StopMove()
        {
            yield return new WaitForSeconds(10f);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }*/
    }
}