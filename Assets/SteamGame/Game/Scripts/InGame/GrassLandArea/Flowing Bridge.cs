using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FlowingBridge : MonoBehaviour
    {
        [SerializeField] GameObject bridge;

        private float spawnTimer;

        private GameObject spawnPoint;

        void Start()
        {
            spawnTimer = 0;
        }

        void Update()
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer <= 0)
            {
                GameObject randamPoint = spawnPoint;
                Instantiate(bridge, randamPoint.transform.position, Quaternion.identity);
                spawnTimer = 1;
            }

        }
    }
}
