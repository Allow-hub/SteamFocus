using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ShotCar : MonoBehaviour
    {
        [Header("ŽÔ‚ÆƒoƒCƒN‚ð”ò‚Î‚·")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool;

        private void OnValidate()
        {
            objectPool = FindObjectOfType<ObjectPool>();
        }
    }
}
