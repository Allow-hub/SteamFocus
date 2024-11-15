using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    public float explosionRadius = 5f;   // �����͈̔�
    public float explosionForce = 10f;   // �����̗�

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Explode();  // �v���C���[���G���Ɣ���
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);  // �����Ő�����΂�
            }
        }

        Destroy(gameObject);  // �n��������
    }
}
