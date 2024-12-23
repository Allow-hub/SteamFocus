using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// プレイヤーがゴールの場所にとどまって数秒立ったらクリア
    /// </summary>
    public class GoalManager : MonoBehaviour
    {
        [SerializeField] private Transform goalCenter;
        [SerializeField] private float goalRadius;      //ターゲットを検出する範囲
        [SerializeField] private GameObject targetObj;  //プレイヤーのオブジェクト
    }
}
