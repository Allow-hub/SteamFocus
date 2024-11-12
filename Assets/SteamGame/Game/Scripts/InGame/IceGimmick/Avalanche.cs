using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : MonoBehaviour
{
    public GameObject snowParticles;   // 雪のエフェクト（Particle Systemなど）
    public float avalancheSpeed = 5.0f; // 雪崩の速さ
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
        snowParticles.SetActive(true);  // 雪のエフェクトを開始

        // 雪崩がプレイヤーに向かって進む
        while (isAvalancheActive)
        {
            snowParticles.transform.position += Vector3.down * avalancheSpeed * Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 雪崩エリアを抜けたら雪崩を停止
        if (other.CompareTag("Player"))
        {
            isAvalancheActive = false;
            snowParticles.SetActive(false);
        }
    }
}
