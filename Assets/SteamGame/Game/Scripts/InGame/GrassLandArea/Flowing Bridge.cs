using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FlowingBridge : MonoBehaviour
    {
        [SerializeField] GameObject bridge;
        [SerializeField] GameObject spawnPoint;

        private float spawnTimer = 0f;


        void Update()
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= 0)
            {
                Instantiate(bridge, spawnPoint.transform.position, Quaternion.identity);
                spawnTimer = 0;
            }

        }
    }
}
