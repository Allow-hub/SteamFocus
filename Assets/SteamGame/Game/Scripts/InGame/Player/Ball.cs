    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float radius = 1.5f;
        [SerializeField] private bool isDrawingGizmo;
        public float Radius => radius;

        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isDrawingGizmo) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
