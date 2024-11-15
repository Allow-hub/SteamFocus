using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : MonoBehaviour
{
    public GameObject snowParticles;   // ��̃G�t�F�N�g�iParticle System�Ȃǁj
    public float avalancheSpeed = 5.0f; // ����̑���
    private bool isAvalancheActive = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAvalancheActive)
        {
            isAvalancheActive = true;
            StartCoroutine(StartAvalanche());
        }
    }

    IEnumerator StartAvalanche()
    {
        snowParticles.SetActive(true);  // ��̃G�t�F�N�g���J�n

        // ������v���C���[�Ɍ������Đi��
        while (isAvalancheActive)
        {
            snowParticles.transform.position += Vector3.down * avalancheSpeed * Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ����G���A�𔲂����������~
        if (other.CompareTag("Player"))
        {
            isAvalancheActive = false;
            snowParticles.SetActive(false);
        }
    }
}
