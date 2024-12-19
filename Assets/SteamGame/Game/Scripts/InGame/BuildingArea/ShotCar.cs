using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ShotCar : MonoBehaviour
    {
        [Header("�Ԃƃo�C�N���΂�")]
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
