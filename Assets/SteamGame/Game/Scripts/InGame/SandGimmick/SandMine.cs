using System.Collections;
using System.Collections.Generic;
using TechC;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;    // �����͈̔�
    [SerializeField] private float explosionForce = 10; // �����̗�

    [SerializeField] private float upwardsModifier = 5; // �����̗�
    [SerializeField] private GameObject mineObj;
    [SerializeField] private GameObject explosionEffect;   // �����̃G�t�F�N�g�i�v���n�u�j
    [SerializeField] private float cameraShakeDuration = 0.5f; // �J�����V�F�C�N�̎���
    [SerializeField] private float cameraShakeMagnitude = 0.2f; // �J�����V�F�C�N�̋��x
    [SerializeField] private float popInterval = 10f;
    [SerializeField] private bool canDraw = false;

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Explode(other.gameObject); // �v���C���[���G���Ɣ���
        }
    }

    void Explode(GameObject obj)
    {
        if (SeManager.I != null)
            SeManager.I.PlaySe(6, 1); //�����̉�
        Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();

        if (rb != null && transform.position != null)
        {
            // �������S����AddExplosionForce�Ŕ���������
            rb.AddExplosionForce(
                explosionForce,
               transform.position,
                explosionRadius,
                upwardsModifier,
                ForceMode.Impulse
            );
        }

        // �J�����V�F�C�N�����s
        //CameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude);
        StartCoroutine(ResetObj());

    }

    private IEnumerator ResetObj()
    {
        mineObj.SetActive(false);
        explosionEffect.SetActive(true);
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(popInterval);
        mineObj.SetActive(true);
        explosionEffect.SetActive(false);
        capsuleCollider.enabled = true;

    }

    private void OnDrawGizmos()
    {
        if (!canDraw) return;
        // Explosion�͈͂������M�Y����`��
        Gizmos.color = Color.red; // �ԐF�ŕ\��
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // �����͈͂����`�ŕ\��
    }
}
