using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackingIceFloor : MonoBehaviour
{
    public float crackTime = 2f;    // ひび割れが始まるまでの時間
    public float breakTime = 4f;    // 完全に割れるまでの時間
    private bool isCracking = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCracking)
        {
            isCracking = true;
            StartCoroutine(CrackAndBreak());
        }
    }

    IEnumerator CrackAndBreak()
    {
        // ひび割れアニメーションの開始
        yield return new WaitForSeconds(crackTime);
        Debug.Log("Ice is cracking!");

        // 割れるまでの遅延
        yield return new WaitForSeconds(breakTime - crackTime);
        Debug.Log("Ice has broken!");
        Destroy(gameObject);
    }
}
