using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TechC
{
    public class DeerGimmick : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 30f;
        private float currentAngle = 0f;
        private bool rotatingFoward = true;


        void Update()
        {
            float step = rotationSpeed * Time.deltaTime;

            if (rotatingFoward)
            {
                currentAngle += step;
                if (currentAngle >= 180f)
                {
                    currentAngle = 180f;
                    rotatingFoward = false;
                }
            }
            else
            {
                currentAngle -= step;
                if (currentAngle <= 0f)
                {
                    currentAngle = 0f;
                    rotatingFoward = true;
                }
            }

            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //‚Ô‚Â‚©‚Á‚½‚©‚Ç‚¤‚©
            if (collision.gameObject.CompareTag("Ball"))
            {
                Debug.Log("”j—ô‚µ‚½");
            }
        }
    }
}
