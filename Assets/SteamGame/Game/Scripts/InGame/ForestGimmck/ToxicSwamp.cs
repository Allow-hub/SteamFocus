using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSwamp : MonoBehaviour
{
    public float damage = 10f;  // 毒沼で受けるダメージ

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // プレイヤーが毒の沼に入った場合
    //        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDamage(damage);  // ダメージを与える
    //            // ゲームオーバー処理もここで行えます
    //        }
    //    }
    //}
}
