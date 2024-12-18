using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class OutWall : MonoBehaviour
    {
        [Header("•ö‚ê—Ž‚¿‚éŠO•Ç")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("ŠO•Ç‚ÌÝ’è")]
        [SerializeField] private float limitTime;
        [SerializeField] private float resetTime;

        private bool isRiding;

        private float elapsedTime = 0;
        private float resetElapsedTime = 0;

        private void Update()
        {
            if (isRiding) return;
            resetElapsedTime += Time.deltaTime;
            if(resetElapsedTime > resetTime)
            {

                resetElapsedTime = 0;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                isRiding = true;
                elapsedTime += Time.deltaTime;
                if(elapsedTime > limitTime)
                {

                    elapsedTime = 0;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                isRiding = false;
            }
        }
    }
}
