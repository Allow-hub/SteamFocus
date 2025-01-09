using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class SnowBall : MonoBehaviour
    {
        [SerializeField] private float duration = 6f;
        private ObjectPool objectPool;
        private void Awake()
        {
            if(objectPool == null ) 
                objectPool = FindAnyObjectByType<ObjectPool>(); 
        }

        private void OnEnable()
        {
            StartCoroutine(DesObj());
        }

        private void OnDisable()
        {
            StopCoroutine(DesObj());
        }

        private IEnumerator DesObj()
        {
            yield return new WaitForSeconds(duration);
            objectPool.ReturnObject(gameObject);
        }
    }
}
