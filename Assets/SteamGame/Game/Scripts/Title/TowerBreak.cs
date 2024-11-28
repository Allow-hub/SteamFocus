using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

namespace TechC
{
    public class TowerBreak : MonoBehaviour
    {
        [SerializeField] private float explosionPower = 1;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionUpwards;
        [SerializeField] private Transform explosionCenter;

        [SerializeField] private bool drawGizmo = false;
        [SerializeField] private Color rangeGizmoColor;
        private void Awake()
        {
            Explosion();
        }

        public void Explosion()
        {
            // ”ÍˆÍ“à‚ÌRigidbody‚ÉAddExplosionForce
            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter.position, explosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                var rb = hitColliders[i].GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddExplosionForce(explosionPower, explosionCenter.position, explosionRadius, explosionUpwards, ForceMode.Impulse);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo) return;
            Gizmos.color = rangeGizmoColor;
            Gizmos.DrawSphere(explosionCenter.position, explosionRadius);

        }
    }
}
