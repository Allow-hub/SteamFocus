using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Ball : MonoBehaviour
    {

        [SerializeField] private Transform outer;

        private void Update()
        {
            transform.position= outer.position;
        }
    }
}
