using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMine : MonoBehaviour
{
    public float explosionRadius = 5f;    // �����͈̔�
    public float explosionForce = 1000f; // �����̗�
    public GameObject explosionEffect;   // �����̃G�t�F�N�g�i�v���n�u�j
    public AudioClip explosionSound;     // �����̃T�E���h
    public float cameraShakeDuration = 0.5f; // �J�����V�F�C�N�̎���
    public float cameraShakeMagnitude = 0.2f; // �J�����V�F�C�N�̋��x

    private AudioSource audioSource;

    void Start()
    {
        // �I�[�f�B�I�\�[�X��ݒ�
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Explode(); // �v���C���[���G���Ɣ���
        }
    }

    void Explode()
    {
        // �����G�t�F�N�g�𐶐�
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // ���������Đ�
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // �����̉e����^����
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius); // �����Ő�����΂�
            }
        }

        // �J�����V�F�C�N�����s
        CameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude);

        // �n��������
        Destroy(gameObject);
    }
}
